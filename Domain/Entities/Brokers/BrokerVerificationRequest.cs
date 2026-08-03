using Domain.Entities.UsersEnities;
using Domain.Enums;

namespace Domain.Entities.Brokers
{
    public class BrokerVerificationRequest
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string LicenseNumber { get; set; }
        public string LicenseImageUrl { get; set; }
        public DateTimeOffset? LicenseExpiryDate { get; set; }

        public VerificationStatus Status { get; set; } = VerificationStatus.Pending;
        public string? RejectionReason { get; set; }  // if admin rejects, explain why

        public Guid? ReviewedByAdminId { get; set; }  // which admin acted on it
        public User? ReviewedByAdmin { get; set; }
        public DateTimeOffset? ReviewedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
