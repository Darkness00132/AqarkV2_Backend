using Application.Constants;
using Application.DTOs.Reviews;
using Application.Exceptions;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly BrokerReviewService _reviewService;

        public ReviewsController(BrokerReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("all/{slug}")]
        public async Task<ActionResult<List<Review>>> GetReviewsByBrokerSlug(string slug)
        {
            var reviews = await _reviewService.GetReviewsByBrokerSlugAsync(slug);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(review);
        }

        [HttpGet("broker/{slug}")]
        public async Task<ActionResult<List<Review>>> GetAllReviewsOfBroker(string slug)
        {
            var reviews = await _reviewService.GetReviewsByBrokerSlugAsync(slug);
            return Ok(reviews);
        }

        [Authorize(Policy = Policies.UserOnly)]
        [HttpPost("{brokerSlug}")]
        public async Task<IActionResult> CreateReview(string brokerSlug, CreateReview dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid reviwerId))
                throw new UnauthorizedException();

            await _reviewService.CreateReviewAsync(reviwerId, brokerSlug, dto);
            return Ok();
        }

        [Authorize(Policy = Policies.UserOnly)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, UpdateReview dto)
        {
            await _reviewService.UpdateReviewAsync(id, dto);
            return Ok();
        }

        [Authorize(Policy = Policies.UserOnly)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return Ok();
        }
    }
}
