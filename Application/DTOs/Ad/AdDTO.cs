using Application.DTOs.User;

namespace Application.DTOs.Ad
{
    public class AdDTO
    {
        public string Slug { get; set; }

        public required string Description { get; set; }

        public int? BathRooms { get; set; }

        public int? Rooms { get; set; }

        public required double Space { get; set; }

        public required int Price { get; set; }

        public required string PropertyAddress { get; set; }

        public required string Type { get; set; }

        public required string PropertyType { get; set; }

        public string? State { get; set; }

        public required BrokerItemList User { get; set; }

        public required string GovernorateName { get; set; }

        public required string CityName { get; set; }

        public required List<ImageDTO> Images { get; set; }

        public required DateTimeOffset CreatedAt { get; set; }
    }
}
