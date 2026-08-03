using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Index(nameof(TransactionId))]
    public class PaymentAttempt
    {
        public Guid Id { get; set; }

        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        [Required]
        [MaxLength(200)]
        public string PaymentId { get; set; }

        [MaxLength(4)]
        public string? CardLast4 { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "numeric(10,2)")]
        public decimal GatewayFee { get; set; }

        public PaymentStatus Status { get; set; }

        [MaxLength(1000)]
        public string? FailureReason { get; set; }

        public DateTimeOffset AttemptedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}