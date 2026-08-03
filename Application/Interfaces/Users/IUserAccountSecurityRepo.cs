using Domain.Entities.UsersEnities;

namespace Application.Interfaces.Users
{
    public interface IUserAccountSecurityRepo
    {

        Task<UserAccountSecurity?> GetByUserIdAsync(Guid userId);

        Task CreateAsync(Guid userId);

        Task BlockUserAsync(Guid userId, Guid blockedByUserId, string reason);
        Task UnblockUserAsync(Guid userId);

        Task UpdateLastLoginAsync(Guid userId, string ipAddress);

        Task<bool> IsUserBlockedAsync(Guid userId);
    }
}
