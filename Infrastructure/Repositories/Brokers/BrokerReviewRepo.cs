using Application.Interfaces.Brokers;
using Domain.Entities.Brokers;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Brokers
{
    public class BrokerReviewRepo : IBrokerReviewRepo
    {
        private readonly AppDbContext _context;

        public BrokerReviewRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BrokerReview review)
        {
            await _context.BrokerReviews.AddAsync(review);
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.BrokerReviews.FindAsync(id);
            if (review is null) return;

            _context.BrokerReviews.Remove(review);
        }

        public async Task<bool> ReviewExistsAsync(Guid brokerUserId, Guid reviewerId)
        {
            return await _context.BrokerReviews
                .AsNoTracking()
                .AnyAsync(r => r.BrokerUserId == brokerUserId && r.UserId == reviewerId);
        }

        public async Task<List<BrokerReview>> GetByBrokerSlugAsync(string slug)
        {
            return await _context.BrokerReviews
                .AsNoTracking()
                .Include(r => r.User)
                .Where(r => r.BrokerProfile.Slug == slug)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<BrokerReview?> GetByIdAsync(int id)
        {
            return await _context.BrokerReviews
                .AsNoTracking()
                .Include(r => r.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BrokerReview?> GetByIdForMutationAsync(int id)
        {
            return await _context.BrokerReviews.FindAsync([id]);
        }

        public async Task<List<BrokerReview>> GetByUserIdAsync(Guid userId)
        {
            return await _context.BrokerReviews
             .AsNoTracking()
             .Include(r => r.User)
             .Where(r => r.UserId == userId)
             .ToListAsync();
        }
    }

}
