using Domain.Entities.UsersEnities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Index(nameof(UserId))]
    public class Transaction
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int CreditsPlanId { get; set; }
        public CreditsPlan Plan { get; set; }

        [Column(TypeName = "numeric(10,2)")]
        public decimal FinalPrice { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}