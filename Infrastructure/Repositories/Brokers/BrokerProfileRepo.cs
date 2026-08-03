
using Application.Interfaces.Brokers;
using Domain.Entities.Brokers;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Brokers
{
    public class BrokerProfileRepo : IBrokerProfileRepo
    {
        private readonly AppDbContext _context;

        public BrokerProfileRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateBrokerProfileAsync(BrokerProfile brokerProfile)
        {
            await _context.BrokerProfiles.AddAsync(brokerProfile);
        }

        public async Task<List<BrokerProfile>> GetAllBrokersProfileAsync()
        {
            return await _context.BrokerProfiles
                .AsNoTracking()
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<BrokerProfile?> GetBrokerProfileByBrokerIdAsync(Guid brokerId, CancellationToken ct = default)
        {
            return await _context.BrokerProfiles
                .AsNoTracking()
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.UserId == brokerId, ct);
        }

        public async Task<BrokerProfile?> GetBrokerProfileByBrokerIdForMutationAsync(Guid brokerId)
        {
            return await _context.BrokerProfiles.FirstOrDefaultAsync(b => b.UserId == brokerId);
        }

        public async Task<BrokerProfile?> GetBrokerProfileBySlugAsync(string slug)
        {
            return await _context.BrokerProfiles
                .AsNoTracking()
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Slug == slug);
        }

        public async Task<Guid?> GetBrokerIdBySlugAsync(string slug)
        {
            return await _context.BrokerProfiles
                .AsNoTracking()
                .Where(b => b.Slug == slug)
                .Select(b => b.UserId)
                .FirstOrDefaultAsync();
        }
    }
}
