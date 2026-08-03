
using Application.DTOs.Brokers;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Brokers;
using Application.Interfaces.ThirdParty;
using AutoMapper;

namespace Application.Services
{
    public class BrokerService
    {
        private readonly IBrokerProfileRepo _brokerProfileRepo;
        private readonly IStorageService _imageService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public BrokerService(IBrokerProfileRepo brokerProfileRepo, IStorageService imageService, IMapper mapper, IUnitOfWork uow)
        {
            _brokerProfileRepo = brokerProfileRepo;
            _imageService = imageService;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<List<BrokerItemList>> GetAllBrokers(CancellationToken ct = default)
        {
            var brokers = await _brokerProfileRepo.GetAllBrokersProfileAsync();
            return _mapper.Map<List<BrokerItemList>>(brokers);
        }

        public async Task<Broker> GetBrokerBySlug(string slug, CancellationToken ct = default)
        {
            var broker = await _brokerProfileRepo.GetBrokerProfileBySlugAsync(slug);
            if (broker is null) throw new NotFoundException("هذا مستخدم غير موجود");
            return _mapper.Map<Broker>(broker);
        }

        public async Task<Broker> GetMyBrokerProfile(Guid brokerId, CancellationToken ct = default)
        {
            var broker = await _brokerProfileRepo.GetBrokerProfileByBrokerIdAsync(brokerId, ct);
            if (broker is null) throw new NotFoundException("هذا مستخدم غير موجود");
            return _mapper.Map<Broker>(broker);
        }

        public async Task UpdateBrokerBrofile(Guid brokerId, UpdateBrokerProfile dto)
        {
            var broker = await _brokerProfileRepo.GetBrokerProfileByBrokerIdForMutationAsync(brokerId);
            if (broker is null) throw new NotFoundException("هذا مستخدم غير موجود");

            if (dto?.CoverPhoto is not null)
            {
                if (broker.CoverPhoto is not null) await _imageService.DeleteOneAsync(broker.CoverPhoto);
                using var stream = dto.CoverPhoto.OpenReadStream();
                broker.CoverPhoto = await _imageService.UploadOneAsync(stream);
            }

            _mapper.Map(dto, broker);
            await _uow.SaveChangesAsync();
        }
    }
}
