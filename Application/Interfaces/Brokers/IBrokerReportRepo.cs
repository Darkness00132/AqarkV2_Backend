using Domain.Entities.Brokers;
using Domain.Enums;

namespace Application.Interfaces.Brokers
{
    public interface IBrokerReportRepo
    {
        Task<BrokerReport?> GetByIdAsync(Guid id);
        Task<BrokerReport?> GetByIdForMutationAsync(Guid id);
        Task<List<BrokerReport>> GetByBrokerIdAsync(Guid brokerUserId);
        Task<List<BrokerReport>> GetByUserIdAsync(Guid userId);
        Task<List<BrokerReport>> GetByStatusAsync(ReportStatus status);
        Task AddAsync(BrokerReport report);
    }
}
