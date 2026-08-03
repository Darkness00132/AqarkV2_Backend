using Application.Common.Filters;
using Domain.Entities.AdEntities;

namespace Infrastructure.Presistance.Extensions
{
    public static class AdQueryExtensions
    {
        public static IQueryable<Ad> ApplyFilters(this IQueryable<Ad> query, AdFilters? filters)
        {
            if (filters is null) return query;

            if (filters.GovernorateId.HasValue)
                query = query.Where(a => a.GovernorateId == filters.GovernorateId);

            if (filters.CityId.HasValue)
                query = query.Where(a => a.CityId == filters.CityId);

            if (filters.Type.HasValue)
                query = query.Where(a => a.Type == filters.Type);

            if (filters.PropertyType.HasValue)
                query = query.Where(a => a.PropertyType == filters.PropertyType);

            if (filters.State.HasValue)
                query = query.Where(a => a.State == filters.State);

            if (filters.MinRooms.HasValue)
                query = query.Where(a => a.Rooms >= filters.MinRooms);


            if (filters.MinPrice.HasValue)
                query = query.Where(a => a.Price >= filters.MinPrice);

            if (filters.MaxPrice.HasValue)
                query = query.Where(a => a.Price <= filters.MaxPrice);

            if (filters.MinSpace.HasValue)
                query = query.Where(a => a.Space >= filters.MinSpace);

            query = filters.SortBy switch
            {
                AdSortBy.PriceAsc => query.OrderBy(a => a.Price),
                AdSortBy.PriceDesc => query.OrderByDescending(a => a.Price),
                AdSortBy.SpaceAsc => query.OrderBy(a => a.Space),
                AdSortBy.SpaceDesc => query.OrderByDescending(a => a.Space),
                _ => query.OrderByDescending(a => a.CreatedAt), // Newest
            };

            return query;
        }
    }
}
