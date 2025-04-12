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

            // Order
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ArtworkName, opt => opt.MapFrom(src => src.Artwork.Name));

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore()) // Items are handled separately
                 .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));
        }
    }
}
