using Application.DTOs.Ad;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

public class RequiredIfPropertyHasRoomsAttribute : ValidationAttribute
{
    private readonly HashSet<PropertyType> _withRooms = new()
    {
        PropertyType.Apartment,
        PropertyType.Villa,
        PropertyType.Chalet,
        PropertyType.House,
        PropertyType.Studio,
        PropertyType.Compound,
    };

    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        var dto = ctx.ObjectInstance as CreateAdDTO;
        if (dto == null) return ValidationResult.Success;

        if (_withRooms.Contains(dto.PropertyType) && (value == null || (int)value == 0))
            return new ValidationResult("عدد الغرف مطلوب لهذا النوع من العقارات");

        return ValidationResult.Success;
    }
}