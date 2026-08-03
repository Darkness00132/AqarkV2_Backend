
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Brokers
{
    public class UpdateBrokerProfile
    {
        public string? Bio { get; set; }

        public string? Phone { get; set; }

        public string? WhatsAppNumber { get; set; }

        public string? ProfilePhoto { get; set; }

        public IFormFile? CoverPhoto { get; set; }

        public string? Address { get; set; }
    }
}
