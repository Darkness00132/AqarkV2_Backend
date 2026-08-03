using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UsersEnities
{
    [Index(nameof(UserId), IsUnique = true)]
    public class UserAccountSecurity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? BlockedByUserId { get; set; }

        [MaxLength(500)]
        public string? BlockReason { get; set; }
        public DateTimeOffset? BlockedAt { get; set; }

        [MaxLength(45)]
        public string? LastLoginIp { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}