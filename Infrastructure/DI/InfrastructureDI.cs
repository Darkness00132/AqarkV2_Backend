using Application.Interfaces;
using Application.Interfaces.Ads;
using Application.Interfaces.Brokers;
using Application.Interfaces.Credits;
using Application.Interfaces.ThirdParty;
using Application.Interfaces.Users;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Ads;
using Infrastructure.Repositories.Brokers;
using Infrastructure.Repositories.Credits;
using Infrastructure.Repositories.Users;
using Infrastructure.Services.ThirdPartyService;
using Infrastructure.ThirdPartyService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration config)
        {
            service.AddDatabase(config);
            service.AddThirdPartyServices(config);

            // Register repositories
            service.AddSingleton<ILocationService, LocationServices>();
            service.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddScoped<IImageRepo, ImageRepo>();
            service.AddScoped<IAdRepo, AdRepo>();
            service.AddScoped<IAdLogRepo, AdLogRepo>();

            service.AddScoped<ICreditsLogRepo, CreditsLogRepo>();
            service.AddScoped<ICreditsRepo, CreditsRepo>();

            service.AddScoped<IBrokerProfileRepo, BrokerProfileRepo>();
            service.AddScoped<IBrokerReviewRepo, BrokerReviewRepo>();
            service.AddScoped<IBrokerReportRepo, BrokerReportRepo>();
            service.AddScoped<IBrokerVerificationRequestRepo, BrokerVerificationRequestRepo>();

            service.AddScoped<IUserRepo, UserRepo>();

            service.AddScoped<IStorageService, S3ImageService>();
            service.AddScoped<IEmailService, ResendEmailService>();
            service.AddScoped<IAccessTokenService, JwtTokenService>();

            return service;
        }
    }
}
