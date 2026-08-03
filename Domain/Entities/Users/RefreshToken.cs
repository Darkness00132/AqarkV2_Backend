using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UsersEnities
{
    [Index(nameof(TokenHash), IsUnique = true)]
    [Index(nameof(UserId))]
    public class RefreshToken
    {
        public int Id { get; set; }

        public required Guid UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [MaxLength(500)]
        public required string TokenHash { get; set; }

        public required DateTime ExpiresAt { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }

        [Required]
        [MaxLength(45)]
        public required string IpAddress { get; set; }
    }
}