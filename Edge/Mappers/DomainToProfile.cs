using AutoMapper;
using Edge.DomainModels;
using Edge.Dtos;

namespace Edge.Mappers
{
    public class DomainToProfile : Profile
    {
        public DomainToProfile() 
        {
            //Artwork
            CreateMap<Artwork, ArtworkDto>();
            CreateMap<ArtworkDto, Artwork>();

            //Cycle
            CreateMap<Cycle, CycleDto>();
            CreateMap<CycleDto, Cycle>();
        }
    }
}
