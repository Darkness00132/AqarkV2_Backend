using Domain.Entities.Brokers;
using Domain.Enums;

namespace Application.Interfaces.Brokers
{
    public interface IBrokerVerificationRequestRepo
    {
        Task<BrokerVerificationRequest?> GetByIdAsync(Guid id);
        Task<BrokerVerificationRequest?> GetByIdForMutationAsync(Guid id);
        Task<BrokerVerificationRequest?> GetByUserIdAsync(Guid userId);         // one active request per broker
        Task<List<BrokerVerificationRequest>> GetByStatusAsync(VerificationStatus status);
        Task<bool> HasPendingRequestAsync(Guid userId);                         // prevent duplicate submissions
        Task AddAsync(BrokerVerificationRequest request);
    }
}
