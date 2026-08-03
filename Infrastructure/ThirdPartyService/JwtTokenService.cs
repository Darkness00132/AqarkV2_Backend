using Application.Exceptions;
using Application.Interfaces.ThirdParty;
using Application.Options;
using Domain.Entities.UsersEnities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.ThirdPartyService
{
    public class JwtTokenService : IAccessTokenService
    {
        readonly private IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateJwtToken(User user, IList<string> roles)
        {
            if (!user.EmailConfirmed)
                throw new UnauthorizedException("يجب تأكيد بريدك الإلكتروني أولاً قبل تسجيل الدخول.");

            if (!user.IsProfileCompleted)
                throw new UnauthorizedException("يرجى إكمال بيانات ملفك الشخصي للمتابعة.");

            var jwtOptions = _config.GetSection("Jwt").Get<JwtSettings>() ?? throw new Exception("Missing JWT configuration section.");

            //add user id in jwt token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                
                // Helpful standard claims
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            //add user roles in jwt token
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.AccessTokenLifetimeMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
