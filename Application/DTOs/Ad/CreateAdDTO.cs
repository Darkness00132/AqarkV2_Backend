using Application.Validators;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Ad
{
    public class CreateAdDTO
    {
        public required string Description { get; set; }

        [RequiredIfPropertyHasRooms]
        public int? Rooms { get; set; }

        [RequiredIfPropertyHasRooms]
        public int? Bathrooms { get; set; }

        [Required]
        public required double Space { get; set; }

        [Required]
        public required int Price { get; set; }

        public required string PropertyAddress { get; set; }

        [Required]
        public required AdType Type { get; set; }

        [RequiredIfHasState]
        public AdState? State { get; set; }

        [Required]
        public required PropertyType PropertyType { get; set; }

        [Required]
        public required int GovernorateId { get; set; }

        [Required]
        public required int CityId { get; set; }

        [AllowImageOnly(MinCount = 1, MaxCount = 5)]
        public required IFormFile[] Images { get; set; }
    }
}
