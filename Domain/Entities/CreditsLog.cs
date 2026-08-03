using Domain.Entities.AdEntities;
using Domain.Entities.UsersEnities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Index(nameof(UserId))]
    [Index(nameof(TransactionId))]
    public class CreditsLog
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? TransactionId { get; set; }
        public Transaction? Transaction { get; set; }

        public Guid? AdId { get; set; }
        public Ad? Ad { get; set; }

        public int Credits { get; set; }

        public CreditsLogAction Action { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}