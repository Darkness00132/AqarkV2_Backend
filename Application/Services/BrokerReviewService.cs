using Application.DTOs.Reviews;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Brokers;
using AutoMapper;
using Domain.Entities.Brokers;

namespace Application.Services
{
    public class BrokerReviewService
    {
        private readonly IBrokerReviewRepo _brokerReviewRepo;
        private readonly IBrokerProfileRepo _brokerProfileRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public BrokerReviewService(IBrokerReviewRepo brokerReviewRepo, IBrokerProfileRepo brokerProfileRepo, IMapper mapper, IUnitOfWork uow)
        {
            _brokerReviewRepo = brokerReviewRepo;
            _brokerProfileRepo = brokerProfileRepo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task CreateReviewAsync(Guid reviewerId, string brokerSlug, CreateReview dto)
        {
            var brokerUserId = await _brokerProfileRepo.GetBrokerIdBySlugAsync(brokerSlug);
            if (brokerUserId is null) throw new NotFoundException("الوسيط غير موجود");

            if (brokerUserId == reviewerId) throw new ConflictException("لا يمكنك مراجعة نفسك");

            bool existingReview = await _brokerReviewRepo.ReviewExistsAsync(brokerUserId.Value, reviewerId);
            if (existingReview) throw new BadRequestException("لقد قمت بمراجعة هذا المستخدم بالفعل");

            var review = _mapper.Map<BrokerReview>(dto);
            review.BrokerUserId = brokerUserId.Value;
            review.UserId = reviewerId;
            await _brokerReviewRepo.AddAsync(review);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            await _brokerReviewRepo.DeleteAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            var review = await _brokerReviewRepo.GetByIdAsync(id);
            return _mapper.Map<Review>(review);
        }

        public async Task<List<ReviewItem>> GetReviewsByBrokerSlugAsync(string slug)
        {
            var reviews = await _brokerReviewRepo.GetByBrokerSlugAsync(slug);
            return _mapper.Map<List<ReviewItem>>(reviews);
        }

        public async Task UpdateReviewAsync(int id, UpdateReview dto)
        {
            var review = await _brokerReviewRepo.GetByIdForMutationAsync(id);
            if (review is null) throw new NotFoundException("هذه المراجعة غير موجودة");
            review.Rating = dto.Rating ?? review.Rating;
            review.Comment = dto.Comment ?? review.Comment;
            await _uow.SaveChangesAsync();
        }
    }
}
