using Domain.Entities.UsersEnities;

namespace Application.Interfaces.Users
{
    public interface IUserRepo
    {
        Task<User?> GetByIdWithBrokerProfileAsync(Guid userId);
    }
}
