
namespace Application.DTOs.User
{
    public class Broker
    {
        public required string Slug { get; set; }

        public required string Name { get; set; }

        public string? Bio { get; set; }

        public string? Phone { get; set; }

        public string? WhatsAppNumber { get; set; }

        public string? ProfilePhoto { get; set; }

        public string? CoverPhoto { get; set; }

        public string? Address { get; set; }

        public bool IsVerified { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }
    }
}
