using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;
using AutoMapper;

namespace AdminPanel.Bll.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GameDto, GameEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<GameEntity, GameDto>()
            .ForMember(dest => dest.GenreIds, opt => opt.Ignore())
            .ForMember(dest => dest.PlatformIds, opt => opt.Ignore());

        CreateMap<GenreEntity, GenreDto>().ReverseMap();
        CreateMap<PlatformEntity, PlatformDto>().ReverseMap();
    }
}