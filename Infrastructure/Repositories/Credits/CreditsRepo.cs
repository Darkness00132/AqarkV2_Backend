using Application.Interfaces.Credits;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Credits
{
    public class CreditsRepo : ICreditsRepo
    {
        private readonly AppDbContext _context;

        public CreditsRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeductAsync(Guid userId, int cost, CancellationToken ct = default)
        {
            var user = await _context.Users
                    .Include(u => u.BrokerProfile)
                    .FirstOrDefaultAsync(u => u.Id == userId, ct);
            if (user is null) return false;
            if (user.BrokerProfile is null) return false;
            if (user.BrokerProfile.Credits < cost) return false;
            user.BrokerProfile.Credits -= cost;
            return true;
        }
    }
}
