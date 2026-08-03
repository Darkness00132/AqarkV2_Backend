using Domain.Enums;

namespace Domain.Services
{
    public class AdCreditCalculator
    {
        private static readonly long[] SalePriceTierBoundaries =
        [
            500_000,
            1_000_000,
            2_000_000,
            5_000_000,
            10_000_000,
        ];

        private static readonly long[] RentPriceTierBoundaries =
        [
            2_000,
            5_000,
            10_000,
            25_000,
            50_000,
        ];

        private static readonly int[] PriceTierBonus = [0, 14, 28, 48, 72, 100];

        private const int FreeUpdateThreshold = 1_000;


        public static int CalculatePostCost(AdType adType, PropertyType propertyType, int price)
        {
            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");

            return GetBaseCredits(adType, propertyType) + PriceTierBonus[GetPriceTier(price, adType)];
        }

        public static int CalculateUpdateCost(int oldPrice, int newPrice, AdType adType, DateTimeOffset createdAt)
        {
            if (oldPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(oldPrice), "Old price must be positive.");
            if (newPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "New price must be positive.");

            // price drop or negligible change — always free
            if (newPrice <= oldPrice) return 0;
            if (newPrice - oldPrice < FreeUpdateThreshold) return 0;

            int oldBonus = PriceTierBonus[GetPriceTier(oldPrice, adType)];
            int newBonus = PriceTierBonus[GetPriceTier(newPrice, adType)];

            // same or lower tier — free
            if (newBonus <= oldBonus) return 0;

            int tierDifference = newBonus - oldBonus;
            double decayFactor = GetAgeDecayFactor(createdAt);

            int cost = (int)Math.Ceiling(tierDifference * decayFactor);

            return Math.Max(1, cost);
        }


        private static double GetAgeDecayFactor(DateTimeOffset createdAt)
        {
            int ageDays = (int)(DateTimeOffset.UtcNow - createdAt).TotalDays;

            return ageDays switch
            {
                <= 7 => 1.00, // week 1 — full cost
                <= 14 => 0.75, // week 2 — 75%
                <= 21 => 0.50, // week 3 — half
                _ => 0.25, // week 4+ — quarter
            };
        }

        private static int GetPriceTier(long price, AdType adType)
        {
            long[] boundaries = adType.IsRent()
                ? RentPriceTierBoundaries
                : SalePriceTierBoundaries;

            for (int i = 0; i < boundaries.Length; i++)
            {
                if (price < boundaries[i])
                    return i;
            }
            return boundaries.Length;
        }

        private static int GetBaseCredits(AdType adType, PropertyType propertyType)
        {
            return propertyType switch
            {
                PropertyType.Apartment or
                PropertyType.Studio => adType switch
                {
                    AdType.RentMonthly => 95,
                    AdType.RentYearly => 95,
                    AdType.RentSeasonal => 106,
                    AdType.SaleCash => 118,
                    AdType.SaleInstallment => 127,
                    _ => 95
                },

                PropertyType.House or
                PropertyType.Chalet => adType switch
                {
                    AdType.RentMonthly => 99,
                    AdType.RentYearly => 97,
                    AdType.RentSeasonal => 110,
                    AdType.SaleCash => 123,
                    AdType.SaleInstallment => 132,
                    _ => 99
                },

                PropertyType.Villa => adType switch
                {
                    AdType.RentMonthly => 111,
                    AdType.RentYearly => 109,
                    AdType.RentSeasonal => 123,
                    AdType.SaleCash => 146,
                    AdType.SaleInstallment => 156,
                    _ => 111
                },

                PropertyType.Compound => adType switch
                {
                    AdType.RentMonthly => 115,
                    AdType.RentYearly => 113,
                    AdType.RentSeasonal => 127,
                    AdType.SaleCash => 151,
                    AdType.SaleInstallment => 161,
                    _ => 115
                },

                PropertyType.CommercialShop or
                PropertyType.Office => adType switch
                {
                    AdType.RentMonthly => 120,
                    AdType.RentYearly => 117,
                    AdType.RentSeasonal => 131,
                    AdType.SaleCash => 157,
                    AdType.SaleInstallment => 167,
                    _ => 120
                },

                PropertyType.MedicalClinic => adType switch
                {
                    AdType.RentMonthly => 133,
                    AdType.RentYearly => 130,
                    AdType.RentSeasonal => 145,
                    AdType.SaleCash => 174,
                    AdType.SaleInstallment => 185,
                    _ => 133
                },

                PropertyType.Hotel => adType switch
                {
                    AdType.RentMonthly => 145,
                    AdType.RentYearly => 142,
                    AdType.RentSeasonal => 159,
                    AdType.SaleCash => 193,
                    AdType.SaleInstallment => 205,
                    _ => 145
                },

                PropertyType.Building => adType switch
                {
                    AdType.RentMonthly => 136,
                    AdType.RentYearly => 133,
                    AdType.RentSeasonal => 148,
                    AdType.SaleCash => 182,
                    AdType.SaleInstallment => 193,
                    _ => 136
                },

                PropertyType.Farm => adType switch
                {
                    AdType.RentMonthly => 106,
                    AdType.RentYearly => 104,
                    AdType.RentSeasonal => 118,
                    AdType.SaleCash => 140,
                    AdType.SaleInstallment => 150,
                    _ => 106
                },

                PropertyType.Land => adType switch
                {
                    AdType.RentMonthly => 102,
                    AdType.RentYearly => 100,
                    AdType.RentSeasonal => 113,
                    AdType.SaleCash => 160,
                    AdType.SaleInstallment => 171,
                    _ => 102
                },

                PropertyType.Warehouse => adType switch
                {
                    AdType.RentMonthly => 104,
                    AdType.RentYearly => 102,
                    AdType.RentSeasonal => 115,
                    AdType.SaleCash => 136,
                    AdType.SaleInstallment => 146,
                    _ => 104
                },

                PropertyType.Garage => adType switch
                {
                    AdType.RentMonthly => 95,
                    AdType.RentYearly => 95,
                    AdType.RentSeasonal => 104,
                    AdType.SaleCash => 115,
                    AdType.SaleInstallment => 123,
                    _ => 95
                },

                _ => adType.IsSale() ? 118 : 95
            };
        }
    }
}