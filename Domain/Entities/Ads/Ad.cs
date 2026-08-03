using Domain.Entities.UsersEnities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.AdEntities
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Ad
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(300)]
        public required string Slug { get; set; }

        [Required]
        [MaxLength(5000)]
        public required string Description { get; set; }

        public int? Rooms { get; set; }

        public int? BathRooms { get; set; }

        public required double Space { get; set; }

        public required int Price { get; set; }

        [Required]
        [MaxLength(500)]
        public required string PropertyAddress { get; set; }

        public required AdType Type { get; set; }

        public required PropertyType PropertyType { get; set; }

        public AdState? State { get; set; }

        public required int GovernorateId { get; set; }

        public required int CityId { get; set; }

        public required Guid UserId { get; set; }

        public User User { get; set; }

        public required ICollection<Image> Images { get; set; } = new List<Image>();

        public ICollection<AdLog> Logs { get; set; } = new List<AdLog>();

        public ICollection<CreditsLog> CreditsLog { get; set; } = new List<CreditsLog>();

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}