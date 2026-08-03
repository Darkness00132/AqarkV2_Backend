
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces.Users;
using AutoMapper;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetMe(string UserId)
        {
            var user = await _userRepo.GetByIdWithBrokerProfileAsync(Guid.Parse(UserId));
            if (user is null) throw new UnauthorizedException();
            return _mapper.Map<UserDTO>(user);
        }
    }
}
