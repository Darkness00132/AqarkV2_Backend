using Application.Interfaces.Users;
using Domain.Entities.UsersEnities;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken token, CancellationToken ct = default)
        {
            await _context.RefreshTokens.AddAsync(token, ct);
        }

        public Task<List<RefreshToken>> GetActiveByUserAsync(Guid userId, CancellationToken ct = default)
        {
            return _context.RefreshTokens
                .Where(rt =>
                    rt.UserId == userId &&
                    rt.RevokedAt == null)
                .ToListAsync(ct);
        }

        public async Task<RefreshToken?> GetByHashAsync(string hash, CancellationToken ct = default)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt =>
                    rt.TokenHash == hash &&
                    rt.RevokedAt == null, ct);

        }
    }
}
