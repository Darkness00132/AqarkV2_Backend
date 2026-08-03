using Application.DTOs.Ad;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

public class RequiredIfHasState : ValidationAttribute
{
    private readonly HashSet<PropertyType> _withState = new()
    {
        PropertyType.Apartment,
        PropertyType.Villa,
        PropertyType.Chalet,
        PropertyType.House,
        PropertyType.Studio,
        PropertyType.Compound,
        PropertyType.Office,
        PropertyType.CommercialShop,
        PropertyType.MedicalClinic,
        PropertyType.Hotel,
        PropertyType.Building,
        PropertyType.Warehouse,
    };

    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        var dto = ctx.ObjectInstance as CreateAdDTO;
        if (dto == null) return ValidationResult.Success;

        if (_withState.Contains(dto.PropertyType) && value == null)
            return new ValidationResult("حالة العقار مطلوب لهذا النوع من العقارات");

        return ValidationResult.Success;
    }
}