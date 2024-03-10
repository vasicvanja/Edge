using AutoMapper;
using Edge.DomainModels;
using Edge.Dtos;

namespace Edge.Mappers
{
    public class DomainToProfile : Profile
    {
        public DomainToProfile() 
        {
            // Login
            CreateMap<Login, LoginDto>();

            // Register
            CreateMap<Register, RegisterDto>();

            // Artwork
            CreateMap<Artwork, ArtworkDto>();
            CreateMap<ArtworkDto, Artwork>();

            // Cycle
            CreateMap<Cycle, CycleDto>();
            CreateMap<CycleDto, Cycle>();

            // SmtpSettings
            CreateMap<SmtpSettings, SmtpSettingsDto>();
            CreateMap<SmtpSettingsDto, SmtpSettings>();
        }
    }
}
