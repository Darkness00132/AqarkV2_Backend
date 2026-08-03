using Application.Constants;
using Application.Options;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplication(this IServiceCollection service, IConfiguration config)
        {
            service.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationDI).Assembly));

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtOptions = config.GetSection("Jwt").Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions!.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                };
            });

            service.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.BrokerOnly, policy =>
                    policy.RequireRole(nameof(UserRoles.Broker)));

                options.AddPolicy(Policies.AdminOnly, policy =>
                    policy.RequireRole(nameof(UserRoles.Admin), nameof(UserRoles.SuperAdmin)));

                options.AddPolicy(Policies.AdminOrBroker, policy =>
                    policy.RequireRole(nameof(UserRoles.SuperAdmin), nameof(UserRoles.Admin), nameof(UserRoles.Broker)));

                options.AddPolicy(Policies.SuperAdminOnly, policy =>
                    policy.RequireRole(nameof(UserRoles.SuperAdmin)));
            });

            service.AddScoped<AuthService>();
            service.AddScoped<AdService>();
            service.AddScoped<BrokerService>();
            service.AddScoped<BrokerReviewService>();
            service.AddScoped<UserService>();

            return service;
        }
    }
}
