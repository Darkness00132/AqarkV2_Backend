using Application.Interfaces.Brokers;
using Domain.Entities.Brokers;
using Domain.Enums;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Brokers
{
    public class BrokerVerificationRequestRepo : IBrokerVerificationRequestRepo
    {
        private readonly AppDbContext _context;

        public BrokerVerificationRequestRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BrokerVerificationRequest?> GetByIdAsync(Guid id)
        {
            return await _context.BrokerVerificationRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<BrokerVerificationRequest?> GetByIdForMutationAsync(Guid id)
        {
            return await _context.BrokerVerificationRequests.FindAsync(id);
        }

        public async Task<BrokerVerificationRequest?> GetByUserIdAsync(Guid userId)
        {
            return await _context.BrokerVerificationRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<List<BrokerVerificationRequest>> GetByStatusAsync(VerificationStatus status)
        {
            return await _context.BrokerVerificationRequests
                .AsNoTracking()
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<bool> HasPendingRequestAsync(Guid userId)
        {
            return await _context.BrokerVerificationRequests
                .AnyAsync(r => r.UserId == userId && r.Status == VerificationStatus.Pending);
        }

        public async Task AddAsync(BrokerVerificationRequest request)
        {
            await _context.BrokerVerificationRequests.AddAsync(request);
        }
    }
}
