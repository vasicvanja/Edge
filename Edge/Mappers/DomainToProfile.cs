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
            CreateMap<CreateCycleDto, Cycle>()
                .ForMember(dest => dest.Artworks, opt => opt.Ignore());

            // SmtpSettings
            CreateMap<SmtpSettings, SmtpSettingsDto>();
            CreateMap<SmtpSettingsDto, SmtpSettings>();

            // Message
            CreateMap<ContactMessage, ContactMessageDto>();
            CreateMap<ContactMessageDto, ContactMessage>();
        }
    }
}
