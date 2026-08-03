using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Brokers;
using Application.Interfaces.Credits;
using Application.Interfaces.ThirdParty;
using Application.Interfaces.Users;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Brokers;
using Domain.Entities.UsersEnities;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICreditsLogRepo _creditsLogRepo;
        private readonly IBrokerProfileRepo _brokerProfileRepo;
        private readonly IAccessTokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, ICreditsLogRepo creditsLogRepo, IBrokerProfileRepo brokerProfileRepo, IAccessTokenService tokenService, IEmailService emailService, IMapper mapper, IRefreshTokenRepo refreshTokenRepo, IUnitOfWork uow, IConfiguration config)
        {
            _userManager = userManager;
            _creditsLogRepo = creditsLogRepo;
            _brokerProfileRepo = brokerProfileRepo;
            _tokenService = tokenService;
            _emailService = emailService;
            _mapper = mapper;
            _refreshTokenRepo = refreshTokenRepo;
            _uow = uow;
            _config = config;
        }

        public async Task ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new NotFoundException("هذا المستخدم غير موجود.");


            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                throw new BadRequestException("رابط تأكيد البريد الإلكتروني غير صالح أو تم نسخه بشكل خاطئ.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded) throw new BadRequestException("تعذر تأكيد البريد الإلكتروني. قد يكون الرابط منتهيًا أو غير صالح.");
        }

        public async Task HandleCompleteProfileAsync(CompleteProfileDTO dto, string? userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedException();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundException("هذا حساب غير موجود");

            await using var transaction = await _uow.BeginTransactionAsync();

            user.IsProfileCompleted = true;
            user.PhoneNumber = dto.PhoneNumber;

            if (dto.Role == PublicRoles.Broker)
            {
                await _brokerProfileRepo.CreateBrokerProfileAsync(new BrokerProfile
                {
                    UserId = user.Id,
                    Slug = GenerateSlug(user.Name),
                    Credits = 100
                });
                await _creditsLogRepo.LogAsync(new CreditsLog
                {
                    UserId = user.Id,
                    Credits = 100,
                    Action = CreditsLogAction.Gift,
                    Description = "create account gift"
                });
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new ConflictException(string.Join(" | ", updateResult.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            if (!roleResult.Succeeded)
                throw new ConflictException(string.Join(" | ", roleResult.Errors.Select(e => e.Description)));

            await _uow.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new BadRequestException("لا يوجد حساب بهذا البريد الإلكتروني.");

            bool isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isConfirmed) throw new ForbiddenException("لا يمكن إعادة تعيين كلمة المرور قبل تأكيد البريد الإلكتروني.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string FrontendUrl = _config.GetValue<string>("FrontendUrl")!;

            string url = $"{FrontendUrl}/resetPassword?email={Uri.EscapeDataString(email)}&token={encodedToken}";

            await _emailService.SendPasswordResetEmailAsync(email, url);
        }

        public async Task<GoogleCallbackResult> HandleGoogleCallbackAsync(AuthenticateResult googleResult)
        {
            string frontendUrl = _config.GetValue<string>("FrontendUrl")!;

            if (!googleResult.Succeeded)
            {
                return GoogleCallbackResult.Failed($"{frontendUrl}/login?error={Encode("فشل تسجيل الدخول عبر Google، يرجى المحاولة مرة أخرى.")}");
            }

            string? email = googleResult.Principal.FindFirstValue(ClaimTypes.Email);
            string? name = googleResult.Principal.FindFirstValue(ClaimTypes.Name)
            ?? $"{googleResult.Principal.FindFirstValue(ClaimTypes.GivenName)} {googleResult.Principal.FindFirstValue(ClaimTypes.Surname)}".Trim();
            string? profilePhoto = googleResult.Principal.FindFirstValue("urn:google:picture");

            if (email == null)
            {
                return GoogleCallbackResult.Failed(
                $"{frontendUrl}/login?error={Encode("لم نتمكن من الحصول على بريدك الإلكتروني من Google، يرجى التحقق من إعدادات حسابك.")}");
            }

            // find or create user
            User? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                //create new user
                user = new User
                {
                    Name = name,
                    UserName = email,
                    Email = email,
                    IsProfileCompleted = false,
                    EmailConfirmed = true,
                    ProfilePhoto = profilePhoto,
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return GoogleCallbackResult.Failed(
              $"{frontendUrl}/login?error={Encode("حدث خطأ أثناء إنشاء حسابك، يرجى المحاولة مرة أخرى.")}");
            }
            else if (user.IsBlocked)
            {
                return GoogleCallbackResult.Failed(
                    $"{frontendUrl}/login?error={Encode("هذا الحساب محظور.")}");
            }

            string refreshToken = await GenerateRefreshToken(user, ip: "Unknown");

            await _uow.SaveChangesAsync();

            int ExpiresAt = _config.GetValue<int>("Jwt:RefreshTokenLifetimeDays");

            return GoogleCallbackResult.Successed(
                user.Id,
                refreshToken,
                DateTime.UtcNow.AddDays(ExpiresAt),
                user.IsProfileCompleted ?
                    $"{frontendUrl}/" :
                    $"{frontendUrl}/auth/completeProfile"
            );
        }

        public async Task<LoginResponse> LoginAsync(LoginDTO userDTO, string ip)
        {
            User? user = await _userManager.FindByEmailAsync(userDTO.Email);

            if (user is null) throw new BadRequestException("بيانات الدخول غير صحيحة.");
            if (user.IsBlocked) throw new ForbiddenException("هذا الحساب محظور.");
            if (!user.EmailConfirmed) throw new ForbiddenException("لم يتم تأكيد البريد الإلكتروني بعد. برجاء مراجعة بريدك وتأكيد الحساب.");

            bool passwordValid = await _userManager.CheckPasswordAsync(user, userDTO.Password);
            if (!passwordValid) throw new BadRequestException("بيانات الدخول غير صحيحة.");

            string accessToken = await CreateAccessToken(user);

            // Generate refresh token
            string rawRefresh = await GenerateRefreshToken(user, ip);
            await _uow.SaveChangesAsync();

            return new LoginResponse { AccessToken = accessToken, RefreshToken = rawRefresh };
        }

        public async Task<LoginResponse> RefreshAsync(string rawRefreshToken, string ip)
        {
            if (string.IsNullOrWhiteSpace(rawRefreshToken))
                throw new UnauthorizedException();

            string hashToken = Hash(rawRefreshToken);

            await using var tx = await _uow.BeginTransactionAsync();

            RefreshToken? stored = await _refreshTokenRepo.GetByHashAsync(hashToken);

            RefreshTokenValidator.Validate(stored);
            if (stored?.User is null) throw new UnauthorizedException();

            // Use a single timestamp for consistency
            var utcNow = DateTime.UtcNow;

            //Revoke old token
            stored.RevokedAt = utcNow;

            //Create new Refresh Token
            string newRefreshToken = await GenerateRefreshToken(stored.User, ip);

            await _uow.SaveChangesAsync();
            await tx.CommitAsync();

            var access = await CreateAccessToken(stored.User);

            return new LoginResponse { RefreshToken = newRefreshToken, AccessToken = access };
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            await using var tx = await _uow.BeginTransactionAsync();

            var user = _mapper.Map<User>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                bool isDuplicateEmail = result.Errors.Any(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName");
                if (isDuplicateEmail)
                    throw new ConflictException("هذا البريد الإلكتروني مسجل بالفعل، يرجى تسجيل الدخول أو استخدام بريد إلكتروني آخر.");

                throw new ConflictException(string.Join(" | ", result.Errors.Select(e => e.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            if (!roleResult.Succeeded)
                throw new ConflictException(string.Join(" | ", roleResult.Errors.Select(e => e.Description)));

            if (dto.Role == PublicRoles.Broker)
            {
                await _brokerProfileRepo.CreateBrokerProfileAsync(new BrokerProfile
                {
                    UserId = user.Id,
                    Slug = GenerateSlug(user.Name),
                    Credits = 100
                });
                await _creditsLogRepo.LogAsync(new CreditsLog
                {
                    UserId = user.Id,
                    Credits = 100,
                    Action = CreditsLogAction.Gift,
                    Description = "create account gift"
                });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            await _uow.SaveChangesAsync();
            await tx.CommitAsync();

            await _emailService.SendValidationEmailAsync(user.Email!, encodedToken, user.Id);
        }

        public async Task ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            User? user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user is null) throw new NotFoundException("لا يوجد حساب بهذه البيانات.");

            string decodedToken;
            try
            {
                var tokenBytes = WebEncoders.Base64UrlDecode(resetPasswordDTO.Token);
                decodedToken = Encoding.UTF8.GetString(tokenBytes);
            }
            catch
            {
                throw new BadRequestException("رابط إعادة تعيين كلمة المرور غير صالح أو تم نسخه بشكل خاطئ.");
            }

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDTO.NewPassword);

            if (!result.Succeeded) throw new BadRequestException(string.Join(" | ", result.Errors.Select(e => e.Description)));

            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task RevokeAllAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var uid))
                throw new BadRequestException("معرّف المستخدم غير صحيح.");

            var tokens = await _refreshTokenRepo.GetActiveByUserAsync(uid);
            if (tokens.Count == 0) return;

            var now = DateTime.UtcNow;
            foreach (var t in tokens) t.RevokedAt = now;

            await _uow.SaveChangesAsync();
        }

        public async Task RevokeCurrentAsync(string rawRefreshToken)
        {
            if (string.IsNullOrWhiteSpace(rawRefreshToken))
                return;

            var hash = Hash(rawRefreshToken);
            var stored = await _refreshTokenRepo.GetByHashAsync(hash);
            if (stored is null) return;

            stored.RevokedAt = DateTime.UtcNow;
            await _uow.SaveChangesAsync();
        }

        private string Hash(string raw)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(raw)));

        }

        private async Task<string> CreateAccessToken(User user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            string accessToken = _tokenService.CreateJwtToken(user, roles);
            return accessToken;
        }

        private async Task<string> GenerateRefreshToken(User user, string ip)
        {
            string rawRefresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            string hash = Hash(rawRefresh);
            int lifetimeDays = _config.GetValue<int>("Jwt:RefreshTokenLifetimeDays");

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddDays(lifetimeDays),
                CreatedAt = DateTime.UtcNow,
                IpAddress = ip
            };
            await _refreshTokenRepo.AddAsync(refreshToken);

            return rawRefresh;
        }

        private string GenerateSlug(string name)
        {
            string slug = name ?? "user";

            // remove tashkeel
            slug = Regex.Replace(slug, @"[\u0610-\u061A\u064B-\u065F]", "");

            // replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // keep arabic letters, latin letters, numbers and hyphens
            slug = Regex.Replace(slug, @"[^\u0600-\u06FF\w-]", "");

            // remove duplicate hyphens
            slug = Regex.Replace(slug, @"-+", "-").Trim('-');

            return slug + "-" + Guid.NewGuid().ToString()[..8];
        }
        private static string Encode(string message) => Uri.EscapeDataString(message);
    }
}