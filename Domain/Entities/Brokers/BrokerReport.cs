using Domain.Entities.UsersEnities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Brokers
{
    [Index(nameof(BrokerUserId), nameof(UserId), IsUnique = true)]
    public class BrokerReport
    {
        public Guid Id { get; set; }

        public Guid BrokerUserId { get; set; }
        public User Broker { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public required ReportReason Reason { get; set; }
        public required string Description { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Pending;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
