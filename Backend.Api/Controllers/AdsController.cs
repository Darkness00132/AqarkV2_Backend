using Application.Common.Filters;
using Application.Common.Pagination;
using Application.Constants;
using Application.DTOs.Ad;
using Application.DTOs.Ad.Private;
using Application.Exceptions;
using Application.Services;
using Domain.Entities.AdEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly AdService _adService;

        public AdsController(AdService adService)
        {
            _adService = adService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PaginationResult<List<AdListItemDTO>>>> GetAllAds([FromQuery] AdFilters filters, [FromQuery] Pagination pagination, CancellationToken ct)
        {
            return await _adService.GetAllAds(filters, pagination, ct);
        }


        [Authorize(Policy = Policies.BrokerOnly)]
        [HttpGet("me")]
        public async Task<ActionResult<PaginationResult<List<AdPrivateListItemDTO>>>> GetMyAds([FromQuery] AdFilters filters, [FromQuery] Pagination pagination, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new UnauthorizedException();

            return await _adService.GetMyAds(filters, pagination, parsedUserId, ct);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<AdDTO>> GetAd(string slug, CancellationToken ct)
        {
            return await _adService.GetAdBySlug(slug, ct);
        }

        [HttpGet("me/{id}")]
        public async Task<ActionResult<Ad>> GetAdById(Guid id, CancellationToken ct)
        {
            return await _adService.GetAdById(id, ct);
        }

        [Authorize(Policy = Policies.BrokerOnly)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAd([FromForm] CreateAdDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new UnauthorizedException();

            await _adService.CreateAdAsync(request, parsedUserId);
            return Created();
        }

        [Authorize(Policy = Policies.BrokerOnly)]
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateAd(Guid id, [FromForm] UpdateAdDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new UnauthorizedException();

            await _adService.UpdateAdAsync(id, parsedUserId, dto);
            return Ok();
        }

        [Authorize(Policy = Policies.BrokerOnly)]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAd(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new UnauthorizedException();

            await _adService.DeleteAdAsync(id, parsedUserId);
            return Ok();
        }
    }
}
