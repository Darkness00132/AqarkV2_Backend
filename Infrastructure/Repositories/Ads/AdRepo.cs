using Application.Common.Filters;
using Application.Common.Pagination;
using Application.Interfaces.Ads;
using Domain.Entities.AdEntities;
using Infrastructure.Presistance;
using Infrastructure.Presistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Ads
{
    public class AdRepo : IAdRepo
    {
        private readonly AppDbContext _context;

        public AdRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ad?> GetByIdToMutateAsync(Guid id)
        {
            return await _context.Ads
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Ad?> GetAdBySlugAsync(string slug, CancellationToken ct = default)
        {
            return await _context.Ads
                        .AsNoTracking()
                        .Include(a => a.Images)
                        .Include(a => a.User)
                        .ThenInclude(u => u.BrokerProfile)
                        .FirstOrDefaultAsync(a => a.Slug == slug, ct);
        }

        public async Task<Ad?> GetAdByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Ads
                .AsNoTracking()
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }
        public async Task<List<Ad>> GetAllAsync(AdFilters? filters, Pagination? pagination, CancellationToken ct = default)
        {
            return await _context.Ads
                .AsNoTracking()
                .ApplyFilters(filters)
                .Include(p => p.Images.Take(1))
                .Include(a => a.User)
                .ThenInclude(u => u.BrokerProfile)
                .ApplyPagination(pagination)
                .ToListAsync(ct);
        }

        public async Task<List<Ad>> GetMineAsync(AdFilters? filters, Pagination? pagination, Guid userId, CancellationToken ct = default)
        {
            return await _context.Ads
                  .AsNoTracking()
                  .Where(a => a.UserId == userId)
                  .ApplyFilters(filters)
                  .Include(p => p.Images.Take(1))
                  .Include(a => a.User)
                  .ThenInclude(u => u.BrokerProfile)
                  .ApplyPagination(pagination)
                  .ToListAsync(ct);
        }
        public async Task<Guid> CreateAdAsync(Ad ad)
        {
            ad.Id = Guid.NewGuid();
            await _context.Ads.AddAsync(ad);
            return ad.Id;
        }

        public void DeleteAd(Ad ad)
        {
            _context.Ads.Remove(ad);
        }

        public async Task<int> Count(CancellationToken ct = default)
        {
            return await _context.Ads.CountAsync();
        }
    }
}
