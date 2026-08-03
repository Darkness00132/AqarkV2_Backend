using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Backend.Api.Controllers
{
    [EnableRateLimiting("Auth")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, IHostEnvironment env, IConfiguration config)
        {
            _authService = authService;
            _env = env;
            _config = config;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallBack), "Auth")
            };

            return Challenge(props, "Google");
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallBack()
        {
            var result = await HttpContext.AuthenticateAsync("Google");

            var response = await _authService.HandleGoogleCallbackAsync(result);

            // Set refresh cookie
            if (response.RefreshToken is not null)
            {
                var options = GetCookiesOptions();
                Response.Cookies.Append("refreshToken", response.RefreshToken, options);
            }

            if (response.UserId is not null)
            {
                Response.Cookies.Append("pending_user_id", response.UserId.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    MaxAge = TimeSpan.FromMinutes(15), // short-lived — only needed to complete profile
                });
            }

            return Redirect(response.RedirectUrl);
        }

        [HttpPost("completeProfile")]
        public async Task<IActionResult> CompleteProfile(CompleteProfileDTO user)
        {
            //get user id from cookie
            string? userId = Request.Cookies["pending_user_id"];
            await _authService.HandleCompleteProfileAsync(user, userId);
            Response.Cookies.Delete("pending_user_id");
            return Ok();
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknow";
            LoginResponse response = await _authService.LoginAsync(userDTO, ip);

            var cookieOptions = GetCookiesOptions();
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

            return Ok(new { accessToken = response.AccessToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            await _authService.RegisterAsync(user);
            return NoContent();
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            await _authService.ConfirmEmailAsync(userId, token);
            return Ok();
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            await _authService.ForgetPassword(email);
            return Ok();
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            await _authService.ResetPassword(resetPasswordDTO);
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                throw new UnauthorizedException();

            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknow";
            LoginResponse response = await _authService.RefreshAsync(refreshToken, ip);

            var cookieOptions = GetCookiesOptions();
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

            return Ok(new { accessToken = response.AccessToken });
        }

        [HttpDelete("revokeCurrent")]
        public async Task<IActionResult> RevokeCurrentToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return Unauthorized();

            await _authService.RevokeCurrentAsync(refreshToken);
            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }

        [Authorize]
        [HttpDelete("revokeAll")]
        public async Task<IActionResult> RevokeAllToken(string userId)
        {
            await _authService.RevokeAllAsync(userId);
            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }

        private CookieOptions GetCookiesOptions(bool sameSite = true)
        {
            var expiresAt = DateTimeOffset.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenLifetimeDays"));

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                //if backend and frontend in different domain
                //SameSite = SameSiteMode.None,
                SameSite = SameSiteMode.Strict,
                Expires = expiresAt,
            };
            return cookieOptions;
        }
    }
}
