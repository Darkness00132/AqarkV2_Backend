namespace Domain.Enums
{
    public enum PropertyType
    {
        Apartment,
        Villa,
        Chalet,
        House,
        Studio,
        Compound,
        Office,
        CommercialShop,
        MedicalClinic,
        Hotel,
        Building,
        Farm,
        Land,
        Warehouse,
        Garage,
    }

    public static class PropertyTypeExtensions
    {
        public static string ToArabic(this PropertyType propertyType) => propertyType switch
        {
            PropertyType.Apartment => "شقة",
            PropertyType.Villa => "فيلا",
            PropertyType.Chalet => "شاليه",
            PropertyType.House => "منزل",
            PropertyType.Studio => "استوديو",
            PropertyType.Compound => "كمبوند",
            PropertyType.Office => "مكتب",
            PropertyType.CommercialShop => "محل تجاري",
            PropertyType.MedicalClinic => "عيادة طبية",
            PropertyType.Hotel => "فندق",
            PropertyType.Building => "عمارة",
            PropertyType.Farm => "مزرعة",
            PropertyType.Land => "أرض",
            PropertyType.Warehouse => "مخزن",
            PropertyType.Garage => "جراج",
            _ => throw new ArgumentOutOfRangeException(nameof(propertyType), propertyType, null)
        };
    }
}