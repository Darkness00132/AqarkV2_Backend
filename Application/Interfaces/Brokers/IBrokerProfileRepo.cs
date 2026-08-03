using Domain.Entities.Brokers;

namespace Application.Interfaces.Brokers
{
    public interface IBrokerProfileRepo
    {

        Task<List<BrokerProfile>> GetAllBrokersProfileAsync();

        Task<BrokerProfile?> GetBrokerProfileByBrokerIdForMutationAsync(Guid brokerId);

        Task<BrokerProfile?> GetBrokerProfileByBrokerIdAsync(Guid brokerId, CancellationToken ct = default);

        Task<BrokerProfile?> GetBrokerProfileBySlugAsync(string slug);

        Task<Guid?> GetBrokerIdBySlugAsync(string slug);

        Task CreateBrokerProfileAsync(BrokerProfile brokerProfile);
    }
}
