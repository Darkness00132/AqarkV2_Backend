using Application.DTOs.Brokers;
using Application.DTOs.Reviews;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Brokers;

namespace Application.Mapping
{
    public class BrokerMapping : Profile
    {
        public BrokerMapping()
        {
            CreateMap<BrokerProfile, Broker>()
                .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.User.ProfilePhoto))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<BrokerProfile, BrokerItemList>()
                .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.User.ProfilePhoto))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<UpdateBrokerProfile, BrokerProfile>().
                ForMember(dest => dest.CoverPhoto, opt => opt.Ignore());

            CreateMap<BrokerReview, Review>()
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ReviewerProfilePhoto, opt => opt.MapFrom(src => src.User.ProfilePhoto));
        }
    }
}
