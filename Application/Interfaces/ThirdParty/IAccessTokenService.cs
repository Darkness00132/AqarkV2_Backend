
using Domain.Entities.UsersEnities;

namespace Application.Interfaces.ThirdParty
{
    public interface IAccessTokenService
    {
        public string CreateJwtToken(User user, IList<string> roles);
    }
}
