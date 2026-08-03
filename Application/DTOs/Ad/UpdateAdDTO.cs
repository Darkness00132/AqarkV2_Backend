using Application.Validators;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Ad
{
    public class UpdateAdDTO
    {
        public string? Description { get; set; }

        public int? Rooms { get; set; }

        public int? BathRooms { get; set; }

        public double? Space { get; set; }

        public int? Price { get; set; }

        public string? PropertyAddress { get; set; }

        public AdState? State { get; set; }

        public List<int>? DeletedImagesIds { get; set; }

        [AllowImageOnly(MinCount = 1, MaxCount = 5)]
        public IFormFile[]? NewImages { get; set; }
    }
}
