using Domain.Entities;
using Domain.Entities.AdEntities;
using Domain.Entities.Brokers;
using Domain.Entities.UsersEnities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Presistance
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserAccountSecurity> UserAccountSecurities { get; set; }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<AdLog> AdLogs { get; set; }

        public DbSet<CreditsPlan> CreditsPlans { get; set; }
        public DbSet<CreditsLog> CreditsLogs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentAttempt> PaymentAttempts { get; set; }

        public DbSet<BrokerProfile> BrokerProfiles { get; set; }
        public DbSet<BrokerReport> BrokerReports { get; set; }
        public DbSet<BrokerReview> BrokerReviews { get; set; }
        public DbSet<BrokerVerificationRequest> BrokerVerificationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BrokerReport>()
                .HasOne(r => r.Broker)
                .WithMany()
                .HasForeignKey(r => r.BrokerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CreditsLog>()
                .HasOne(c => c.Ad)
                .WithMany(a => a.CreditsLog)
                .HasForeignKey(c => c.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
