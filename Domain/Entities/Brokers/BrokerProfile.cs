using Domain.Entities.UsersEnities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Brokers
{
    [Index(nameof(UserId))]
    [Index(nameof(Slug))]
    public class BrokerProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Slug { get; set; }

        public int Credits { get; set; } = 0;

        public string? LicenseNumber { get; set; }
        public DateTimeOffset? LicenseExpiryDate { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? CoverPhoto { get; set; }
        public string? Address { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsFeatured { get; set; } = false;

        public double AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;

        public ICollection<BrokerReview> Reviews { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
