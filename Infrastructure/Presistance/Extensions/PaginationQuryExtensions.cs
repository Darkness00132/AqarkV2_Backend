using Application.Common.Pagination;

namespace Infrastructure.Presistance.Extensions
{
    public static class PaginationQuryExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, Pagination? pagination)
        {
            if (pagination is null) return query;
            return query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }
    }
}
