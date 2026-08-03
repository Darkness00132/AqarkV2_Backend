using Domain.Entities.Brokers;

namespace Application.Interfaces.Brokers
{
    public interface IBrokerReviewRepo
    {
        Task<BrokerReview?> GetByIdForMutationAsync(int id);
        Task<BrokerReview?> GetByIdAsync(int id);
        Task<List<BrokerReview>> GetByBrokerSlugAsync(string slug);
        Task<List<BrokerReview>> GetByUserIdAsync(Guid userId);
        Task<bool> ReviewExistsAsync(Guid brokerUserId, Guid reviewerId);
        Task AddAsync(BrokerReview review);
        Task DeleteAsync(int id);
    }
}
