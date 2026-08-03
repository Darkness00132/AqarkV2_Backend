using Domain.Entities.UsersEnities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CreditsPlan
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int Credits { get; set; }

        [Required]
        [Column(TypeName = "numeric(10,2)")]
        public decimal Price { get; set; }

        public int? Bonus { get; set; }

        public PlanDiscount? Discount { get; set; }
    }
}