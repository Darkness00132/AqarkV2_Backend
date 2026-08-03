using Application.Interfaces.Credits;
using Domain.Entities;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Credits
{
    public class CreditsLogRepo : ICreditsLogRepo
    {
        private readonly AppDbContext _db;

        public CreditsLogRepo(AppDbContext db) => _db = db;

        public async Task LogAsync(CreditsLog log, CancellationToken ct = default)
        {
            await _db.CreditsLogs.AddAsync(log, ct);
        }

        public async Task<List<CreditsLog>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        {
            return await _db.CreditsLogs
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }
    }
}
