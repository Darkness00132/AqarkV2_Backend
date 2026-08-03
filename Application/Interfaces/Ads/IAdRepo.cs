using Application.Common.Filters;
using Application.Common.Pagination;
using Domain.Entities.AdEntities;

namespace Application.Interfaces.Ads
{
    public interface IAdRepo
    {
        Task<int> Count(CancellationToken ct = default);
        Task<List<Ad>> GetAllAsync(AdFilters? filters, Pagination? pagination, CancellationToken ct = default);

        Task<List<Ad>> GetMineAsync(AdFilters? filters, Pagination? pagination, Guid userId, CancellationToken ct = default);

        Task<Ad?> GetAdBySlugAsync(string slug, CancellationToken ct = default);

        Task<Ad?> GetAdByIdAsync(Guid id, CancellationToken ct = default);

        Task<Ad?> GetByIdToMutateAsync(Guid id);

        Task<Guid> CreateAdAsync(Ad ad);

        void DeleteAd(Ad ad);
    }
}
