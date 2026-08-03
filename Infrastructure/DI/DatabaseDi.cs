
using Domain.Entities.UsersEnities;
using Domain.Enums;
using Infrastructure.Presistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
    public static class DatabaseDi
    {
        public static IServiceCollection AddDatabase(this IServiceCollection service, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string cannot be empty");

            service.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString, o =>
                {
                    o.MapEnum<AdType>();
                    o.MapEnum<AdState>();
                    o.MapEnum<PropertyType>();
                    o.MapEnum<AdAction>();
                    o.MapEnum<CreditsLogAction>();
                    o.MapEnum<PaymentStatus>();
                    o.MapEnum<ReportReason>();
                    o.MapEnum<ReportStatus>();
                    o.MapEnum<VerificationStatus>();
                })
            );
            service.AddDataProtection();
            service.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return service;
        }
    }
}
