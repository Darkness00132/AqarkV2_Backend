using Domain.Entities.AdEntities;

namespace Application.Interfaces.Ads
{
    public interface IImageRepo
    {
        Task AddRangeAsync(List<Image> images, CancellationToken ct = default);

        Task<List<Image>> GetByAdIdAsync(Guid adId, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
