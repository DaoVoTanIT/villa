using AutoMapper;
using villa_web.Dto;

namespace villa_web
{
    class MappingConfig : Profile
    {
        public MappingConfig()
        {
        
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberUpdateDto>().ReverseMap();
        }
    }
}

