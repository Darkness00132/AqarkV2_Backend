using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.UsersEnities;

namespace Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<RegisterDTO, User>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<User, UserDTO>().ForMember(dest => dest.Credits, opt => opt.MapFrom(src => src.BrokerProfile.Credits));

            CreateMap<User, Broker>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.BrokerProfile.Slug))
                .ForMember(dest => dest.WhatsAppNumber, opt => opt.MapFrom(src => src.BrokerProfile.WhatsAppNumber))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.BrokerProfile.Phone));

            CreateMap<User, BrokerItemList>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.BrokerProfile.Slug));
        }
    }
}
