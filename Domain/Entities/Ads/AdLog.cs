using Domain.Entities.UsersEnities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.AdEntities
{
    [Index(nameof(AdId))]
    public class AdLog
    {
        public Guid Id { get; set; }

        public Guid AdId { get; set; }
        public Ad Ad { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public AdAction Action { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}