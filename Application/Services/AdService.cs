using Application.Common.Filters;
using Application.Common.Pagination;
using Application.DTOs.Ad;
using Application.DTOs.Ad.Private;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Ads;
using Application.Interfaces.Credits;
using Application.Interfaces.ThirdParty;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.AdEntities;
using Domain.Enums;
using Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class AdService
    {
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IAdRepo _adRepo;
        private readonly IImageRepo _imageRepo;
        private readonly ICreditsRepo _creditsRepo;
        private readonly IAdLogRepo _adLogRepo;
        private readonly ICreditsLogRepo _creditsLogRepo;
        private readonly ILocationService _locationService;
        private readonly IUnitOfWork _uow;

        private readonly IMemoryCache _cache;
        private readonly ILogger<AdService> _logger;

        public AdService(
            IStorageService storageService,
            IMapper mapper,
            IAdRepo adRepo,
            IImageRepo imageRepo,
            ICreditsRepo creditsRepo,
            IAdLogRepo adLogRepo,
            ICreditsLogRepo creditsLogRepo,
            ILocationService locationService,
            IUnitOfWork uow,
            IMemoryCache cache,
            ILogger<AdService> logger)
        {
            _storageService = storageService;
            _mapper = mapper;
            _adRepo = adRepo;
            _imageRepo = imageRepo;
            _creditsRepo = creditsRepo;
            _adLogRepo = adLogRepo;
            _creditsLogRepo = creditsLogRepo;
            _locationService = locationService;
            _uow = uow;
            _cache = cache;
            _logger = logger;
        }

        #region CREATE

        public async Task<Guid> CreateAdAsync(CreateAdDTO dto, Guid userId)
        {
            _logger.LogInformation("CreateAd started for UserId={UserId}", userId);

            if (!_locationService.CityBelongsToGovernorate(dto.GovernorateId, dto.CityId))
                throw new BadRequestException("المدينة مدخلة ليس ضمن نطاق محافظة المدخلة");

            var imagesStreams = dto.Images.Select(x => x.OpenReadStream()).ToList();
            var uploadedUrls = new List<string>();

            await using var transaction = await _uow.BeginTransactionAsync();

            try
            {
                uploadedUrls = await _storageService.UploadManyAsync(imagesStreams);

                int cost = AdCreditCalculator.CalculatePostCost(dto.Type, dto.PropertyType, dto.Price);

                if (!await _creditsRepo.DeductAsync(userId, cost))
                    throw new BadRequestException("لا يوجد كريدت كافية لعمل اعلان");

                var ad = _mapper.Map<Ad>(dto);
                ad.UserId = userId;

                ad.Slug = GenerateSlug(
                    ad,
                    _locationService.GetCityName(ad.GovernorateId, ad.CityId),
                    _locationService.GetGovernorateName(ad.GovernorateId));

                var adId = await _adRepo.CreateAdAsync(ad);

                await _adLogRepo.LogAsync(new AdLog
                {
                    AdId = adId,
                    UserId = userId,
                    Action = AdAction.Created
                });

                await _creditsLogRepo.LogAsync(new CreditsLog
                {
                    UserId = userId,
                    AdId = adId,
                    Credits = -cost,
                    Action = CreditsLogAction.Spend
                });

                var images = uploadedUrls.Select(url => new Image
                {
                    Url = url,
                    AdId = adId
                }).ToList();

                await _imageRepo.AddRangeAsync(images);

                await _uow.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Ad created successfully AdId={AdId}", adId);

                return adId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _storageService.DeleteManyAsync(uploadedUrls);

                _logger.LogError(ex, "CreateAd failed for UserId={UserId}", userId);
                throw;
            }
            finally
            {
                foreach (var s in imagesStreams)
                    await s.DisposeAsync();
            }
        }

        #endregion

        #region READ (WITH CACHE)

        public async Task<AdDTO> GetAdBySlug(string slug, CancellationToken ct = default)
        {
            string cacheKey = $"ad:slug:{slug}";

            if (_cache.TryGetValue(cacheKey, out AdDTO cached))
            {
                _logger.LogInformation("Cache HIT for {Slug}", slug);
                return cached;
            }

            _logger.LogInformation("Cache MISS for {Slug}", slug);

            var ad = await _adRepo.GetAdBySlugAsync(slug);

            if (ad is null)
                throw new NotFoundException("الاعلان غير موجود");

            var result = _mapper.Map<AdDTO>(ad);

            result.GovernorateName = _locationService.GetGovernorateName(ad.GovernorateId);
            result.CityName = _locationService.GetCityName(ad.GovernorateId, ad.CityId);

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });

            return result;
        }

        public async Task<Ad> GetAdById(Guid id, CancellationToken ct = default)
        {
            var ad = await _adRepo.GetAdByIdAsync(id, ct);

            if (ad is null)
                throw new NotFoundException("الاعلان غير موجود");

            return ad;
        }

        public async Task<PaginationResult<List<AdListItemDTO>>> GetAllAds(
            AdFilters? filters,
            Pagination? pagination,
            CancellationToken ct = default)
        {
            var ads = await _adRepo.GetAllAsync(filters, pagination, ct);
            var result = _mapper.Map<List<AdListItemDTO>>(ads);

            for (int i = 0; i < result.Count; i++)
            {
                result[i].GovernorateName = _locationService.GetGovernorateName(ads[i].GovernorateId);
                result[i].CityName = _locationService.GetCityName(ads[i].GovernorateId, ads[i].CityId);
            }

            return new PaginationResult<List<AdListItemDTO>>
            {
                Items = result,
                TotalCount = await _adRepo.Count(),
                Page = pagination?.Page ?? 1,
                PageSize = pagination?.PageSize ?? 12
            };
        }

        public async Task<PaginationResult<List<AdPrivateListItemDTO>>> GetMyAds(
            AdFilters? filters,
            Pagination? pagination,
            Guid userId,
            CancellationToken ct = default)
        {
            var ads = await _adRepo.GetMineAsync(filters, pagination, userId, ct);
            var result = _mapper.Map<List<AdPrivateListItemDTO>>(ads);

            for (int i = 0; i < ads.Count; i++)
            {
                result[i].GovernorateName = _locationService.GetGovernorateName(ads[i].GovernorateId);
                result[i].CityName = _locationService.GetCityName(ads[i].GovernorateId, ads[i].CityId);
            }

            return new PaginationResult<List<AdPrivateListItemDTO>>
            {
                Items = result,
                TotalCount = await _adRepo.Count(),
                Page = pagination?.Page ?? 1,
                PageSize = pagination?.PageSize ?? 12
            };
        }

        #endregion

        #region UPDATE

        public async Task UpdateAdAsync(Guid id, Guid userId, UpdateAdDTO dto)
        {
            var ad = await _adRepo.GetByIdToMutateAsync(id);

            if (ad is null)
                throw new NotFoundException("هذا الاعلان غير متاح");

            if (ad.UserId != userId)
                throw new ForbiddenException("لا تمتلك صلاحية تعديل على هذا الاعلان");

            _mapper.Map(dto, ad);

            await _uow.SaveChangesAsync();

            // invalidate cache
            _cache.Remove($"ad:slug:{ad.Slug}");

            _logger.LogInformation("Ad updated AdId={AdId}", id);
        }

        #endregion

        #region DELETE

        public async Task DeleteAdAsync(Guid id, Guid userId)
        {
            var ad = await _adRepo.GetByIdToMutateAsync(id);

            if (ad is null)
                throw new NotFoundException("الاعلان غير موجود");

            if (ad.UserId != userId)
                throw new ForbiddenException("لا تمتلك صلاحية تعديل على هذا الاعلان");

            var imagesToDelete = ad.Images.Select(x => x.Url).ToList();

            _adRepo.DeleteAd(ad);

            await _adLogRepo.LogAsync(new AdLog
            {
                AdId = id,
                UserId = userId,
                Action = AdAction.Delete
            });

            await _uow.SaveChangesAsync();

            _cache.Remove($"ad:slug:{ad.Slug}");

            await _storageService.DeleteManyAsync(imagesToDelete);

            _logger.LogWarning("Ad deleted AdId={AdId}", id);
        }

        #endregion

        #region HELPERS

        private static string GenerateSlug(Ad ad, string city, string governorate)
        {
            var slug = string.Join("-", ad.PropertyType.ToArabic(), ad.Type.ToArabic(), city, governorate);

            slug = Regex.Replace(slug, @"[\u0610-\u061A\u064B-\u065F]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^\u0600-\u06FF-]", "");
            slug = Regex.Replace(slug, @"-+", "-").Trim('-');

            return slug + "-" + Guid.NewGuid().ToString()[..8];
        }

        #endregion
    }
}