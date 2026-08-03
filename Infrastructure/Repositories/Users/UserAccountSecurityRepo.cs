using Application.Interfaces.Users;
using Domain.Entities.UsersEnities;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users
{
    public class UserAccountSecurityRepo : IUserAccountSecurityRepo
    {
        private readonly AppDbContext _context;

        public UserAccountSecurityRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task BlockUserAsync(Guid userId, Guid blockedByUserId, string reason)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));

            if (blockedByUserId == Guid.Empty)
                throw new ArgumentException("blockedByUserId cannot be empty.", nameof(blockedByUserId));

            // Normalize/trim reason and respect [MaxLength(500)]
            var normalizedReason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();
            if (normalizedReason?.Length > 500)
                normalizedReason = normalizedReason.Substring(0, 500);

            // Ensure record exists
            var UserSecurity = await _context.UserAccountSecurities
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (UserSecurity is null)
            {
                // Optional: verify user exists
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                    throw new InvalidOperationException($"User with Id '{userId}' does not exist.");

                UserSecurity = new UserAccountSecurity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                };

                _context.UserAccountSecurities.Add(UserSecurity);
            }

            UserSecurity.BlockedByUserId = blockedByUserId;
            UserSecurity.BlockReason = normalizedReason;
            UserSecurity.BlockedAt = DateTimeOffset.UtcNow;
            UserSecurity.UpdatedAt = DateTimeOffset.UtcNow;
        }


        /// <summary>
        /// Creates a UserAccountSecurity row for a user if not exists.
        /// </summary>

        public async Task CreateAsync(Guid userId)
        {
            var existing = await _context.UserAccountSecurities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == userId);


            if (existing != null)
                return;

            // Optional: verify the user exists to avoid FK violation
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new InvalidOperationException($"User with Id '{userId}' does not exist.");


            var entity = new UserAccountSecurity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
            };
            _context.UserAccountSecurities.Add(entity);
        }

        public async Task<UserAccountSecurity?> GetByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));

            return await _context.UserAccountSecurities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> IsUserBlockedAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));

            return await _context.UserAccountSecurities
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.BlockedAt != null);

        }

        public async Task UnblockUserAsync(Guid userId)
        {

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));

            var UserSecurity = await _context.UserAccountSecurities
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (UserSecurity is null || UserSecurity.BlockedAt is null) return;


            UserSecurity.BlockedByUserId = null;
            UserSecurity.BlockReason = null;
            UserSecurity.BlockedAt = null;
            UserSecurity.UpdatedAt = DateTimeOffset.UtcNow;
        }

        public async Task UpdateLastLoginAsync(Guid userId, string ipAddress)
        {

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));

            // Normalize IP and respect [MaxLength(45)] (enough for IPv6 with scope)
            var ip = string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress.Trim();
            if (ip?.Length > 45)
                ip = ip.Substring(0, 45);

            var security = await _context.UserAccountSecurities
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (security is null)
            {
                // Optional: verify user exists
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);

                if (!userExists)
                    throw new InvalidOperationException($"User with Id '{userId}' does not exist.");

                security = new UserAccountSecurity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                };
                _context.UserAccountSecurities.Add(security);
            }

            security.LastLoginIp = ip;
            security.LastLoginAt = DateTimeOffset.UtcNow;
            security.UpdatedAt = DateTimeOffset.UtcNow;

        }
    }
}
