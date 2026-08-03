using Domain.Entities.UsersEnities;

namespace Application.Interfaces.Users
{
    public interface IRefreshTokenRepo
    {
        Task AddAsync(RefreshToken token, CancellationToken ct = default);
        Task<RefreshToken?> GetByHashAsync(string hash, CancellationToken ct = default);
        Task<List<RefreshToken>> GetActiveByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
