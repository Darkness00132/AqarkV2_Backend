using Domain.Entities;

namespace Application.Interfaces.Credits
{
    public interface ICreditsLogRepo
    {
        Task LogAsync(CreditsLog log, CancellationToken ct = default);
        Task<List<CreditsLog>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
