namespace Domain.Enums
{
    public enum AdType
    {
        RentMonthly,
        RentYearly,
        RentSeasonal,
        SaleCash,
        SaleInstallment,
    }

    public static class AdTypeExtensions
    {
        public static string ToArabic(this AdType adType) => adType switch
        {
            AdType.RentMonthly => "إيجار شهري",
            AdType.RentYearly => "إيجار سنوي",
            AdType.RentSeasonal => "إيجار موسمي",
            AdType.SaleCash => "بيع كاش",
            AdType.SaleInstallment => "بيع بالتقسيط",
            _ => throw new ArgumentOutOfRangeException(nameof(adType), adType, null)
        };

        public static bool IsRent(this AdType adType) =>
            adType is AdType.RentMonthly or AdType.RentYearly or AdType.RentSeasonal;

        public static bool IsSale(this AdType adType) =>
            adType is AdType.SaleCash or AdType.SaleInstallment;
    }
}