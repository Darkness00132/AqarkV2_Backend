using Domain.Entities.Brokers;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UsersEnities
{
    public class User : IdentityUser<Guid>
    {

        public required string Name { get; set; }

        public string? ProfilePhoto { get; set; }

        public bool IsProfileCompleted { get; set; } = false;

        public bool IsBlocked { get; set; } = false;

        public UserAccountSecurity? Security { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<BrokerReview> Reviews { get; set; }
        public ICollection<BrokerReport> Reports { get; set; }

        public BrokerProfile? BrokerProfile { get; set; }
    }
}
