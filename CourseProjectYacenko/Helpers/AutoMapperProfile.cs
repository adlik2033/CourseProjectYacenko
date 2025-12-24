using AutoMapper;
using CourseProjectYacenko.DTO;
using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;

namespace CourseProjectYacenko.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // AppUser mappings
            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // Tariff mappings
            CreateMap<Tariff, TariffDto>();
            CreateMap<Service, ServiceDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            // Payment mappings
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Application mappings
            CreateMap<Application, ApplicationDto>()
                .ForMember(dest => dest.SubscriberName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}