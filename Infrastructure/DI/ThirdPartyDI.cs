using Amazon.S3;
using Application.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resend;

namespace Infrastructure.DI
{
    public static class ThirdPartyDI
    {
        public static IServiceCollection AddThirdPartyServices(this IServiceCollection service, IConfiguration config)
        {
            service.AddOptions();

            service.AddHttpClient<ResendClient>();
            service.Configure<ResendClientOptions>(opt => opt.ApiToken = config.GetValue<string>("EmailService:ApiKey")!);
            service.AddTransient<IResend, ResendClient>();

            service.Configure<S3Settings>(config.GetSection("S3Settings"));
            service.AddSingleton<IAmazonS3>(sp =>
            {
                var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
                var s3Config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(s3Settings.Region)
                };

                if (!string.IsNullOrWhiteSpace(s3Settings.AccessKey) &&
                    !string.IsNullOrWhiteSpace(s3Settings.SecretKey))
                {
                    return new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, s3Config);
                }

                return new AmazonS3Client(s3Config);
            });

            service.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = config.GetValue<string>("Google:ClientId")!;
                options.ClientSecret = config.GetValue<string>("Google:ClientSecret")!;
                options.CallbackPath = "/signin-google";
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            });

            return service;
        }
    }
}
