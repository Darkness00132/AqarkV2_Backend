using Application.DTOs.Ad;
using Application.DTOs.Ad.Private;
using AutoMapper;
using Domain.Entities.AdEntities;
using Domain.Enums;

namespace Application.Mapping
{
    public class AdMapping : Profile
    {
        public AdMapping()
        {
            CreateMap<Image, ImageDTO>().ReverseMap();

            CreateMap<Ad, AdDTO>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToArabic()))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.ToArabic()))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.HasValue ? src.State.Value.ToArabic() : null));

            CreateMap<UpdateAdDTO, Ad>();

            CreateMap<CreateAdDTO, Ad>().ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Ad, AdListItemDTO>()
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src => src.Images.FirstOrDefault()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToArabic()))
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.ToArabic()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.HasValue ? src.State.Value.ToArabic() : null));

            CreateMap<Ad, AdPrivateListItemDTO>()
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src => src.Images.FirstOrDefault()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToArabic()))
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.ToArabic()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.HasValue ? src.State.Value.ToArabic() : null));

        }
    }
}
