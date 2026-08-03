using Application.Interfaces.Brokers;
using Domain.Entities.Brokers;
using Domain.Enums;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Brokers
{
    public class BrokerReportRepo : IBrokerReportRepo
    {
        private readonly AppDbContext _context;
        public BrokerReportRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BrokerReport report)
        {
            await _context.BrokerReports.AddAsync(report);
        }

        public async Task<List<BrokerReport>> GetByBrokerIdAsync(Guid brokerUserId)
        {
            return await _context.BrokerReports
                .AsNoTracking()
                .Where(b => b.BrokerUserId == brokerUserId)
                .ToListAsync();
        }

        public async Task<BrokerReport?> GetByIdAsync(Guid id)
        {
            return await _context.BrokerReports
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<BrokerReport?> GetByIdForMutationAsync(Guid id)
        {
            return await _context.BrokerReports.FindAsync([id]);
        }

        public async Task<List<BrokerReport>> GetByStatusAsync(ReportStatus status)
        {
            return await _context.BrokerReports
                .AsNoTracking()
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task<List<BrokerReport>> GetByUserIdAsync(Guid userId)
        {
            return await _context.BrokerReports.AsNoTracking().Where(b => b.UserId == userId).ToListAsync();
        }
    }
}
