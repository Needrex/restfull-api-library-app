using AutoMapper;
using RestApiApp.Dtos;
using RestApiApp.Models;

namespace RestApiApp.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Users, RegisterResponeDto>().ReverseMap();
            CreateMap<Users, RegisterRequestDto>().ReverseMap();

            CreateMap<Users, LoginResponeDto >()
                            .ForMember(
                                dest => dest.RefreshToken,
                                opt => opt.MapFrom(src => src.RefreshToken.Token))
                                .ReverseMap();

            CreateMap<Users, LogoutResponeDto>()
                            .ForMember(
                                dest => dest.RefreshToken,
                                opt => opt.MapFrom(src => src.RefreshToken.Token))
                                .ReverseMap();


            CreateMap<Users, AccessTokenRequestDto>()
                            .ForMember(
                                dest => dest.UserId,
                                opt => opt.MapFrom(src => src.Id))
                                .ReverseMap();
        }
    }

}