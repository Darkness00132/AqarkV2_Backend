using Application.Interfaces.Ads;
using Domain.Entities.AdEntities;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Ads
{
    public class AdLogRepo : IAdLogRepo
    {
        private readonly AppDbContext _context;

        public AdLogRepo(AppDbContext context) => _context = context;

        public async Task<List<AdLog>> GetByAdId(Guid adId, CancellationToken ct = default)
        {
            return await _context.AdLogs.Where(a => a.AdId == adId).ToListAsync(ct);
        }

        public async Task LogAsync(AdLog log, CancellationToken ct = default)
        {
            await _context.AdLogs.AddAsync(log, ct);
        }
    }
}
