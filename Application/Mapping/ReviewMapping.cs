using Application.DTOs.Reviews;
using AutoMapper;
using Domain.Entities.Brokers;

namespace Application.Mapping
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            CreateMap<BrokerReview, Review>()
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ReviewerProfilePhoto, opt => opt.MapFrom(src => src.User.ProfilePhoto));

            CreateMap<BrokerReview, ReviewItem>()
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ReviewerProfilePhoto, opt => opt.MapFrom(src => src.User.ProfilePhoto));

            CreateMap<CreateReview, BrokerReview>();

            CreateMap<UpdateReview, BrokerReview>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
