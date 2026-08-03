using Application.Exceptions;
using Domain.Entities.UsersEnities;

namespace Application.Validators
{
    public static class RefreshTokenValidator
    {
        public static void Validate(RefreshToken? token)
        {
            if (token is null || token.RevokedAt is not null || token.ExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedException();
        }
    }
}
