using AutoMapper;
using Villa_API.Dto;
using Villa_API.Model;

namespace Villa_API
{
    class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDto>();
            CreateMap<VillaNumberDto, VillaNumber>();
            CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();
        }
    }
}

