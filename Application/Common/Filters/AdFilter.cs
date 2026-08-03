
using Domain.Enums;

namespace Application.Common.Filters
{
    public class AdFilters
    {
        public int? GovernorateId { get; set; }

        public int? CityId { get; set; }

        public AdType? Type { get; set; }

        public PropertyType? PropertyType { get; set; }

        public AdState? State { get; set; }

        public int? MinRooms { get; set; }

        public int? minBathRooms { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public double? MinSpace { get; set; }

        public AdSortBy SortBy { get; set; } = AdSortBy.Newest;
    }

    public enum AdSortBy
    {
        Newest,
        PriceAsc,
        PriceDesc,
        SpaceAsc,
        SpaceDesc,
    }
}
