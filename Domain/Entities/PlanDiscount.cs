using Domain.Entities.UsersEnities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Index(nameof(CreditsPlanId), IsUnique = true)]
    public class PlanDiscount
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int CreditsPlanId { get; set; }
        public CreditsPlan CreditsPlan { get; set; }

        [Range(1, 100)]
        public int Percentage { get; set; }

        public DateTimeOffset StartAt { get; set; }

        public DateTimeOffset EndsAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}