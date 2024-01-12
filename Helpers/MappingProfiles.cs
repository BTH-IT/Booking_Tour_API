using AutoMapper;
using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account))
                .ReverseMap();

            CreateMap<User, UserRequestDTO>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account.Id))
                .ReverseMap();

            CreateMap<Account, AccountResponseDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ReverseMap();

            CreateMap<Role, RoleResponseDTO>()
                .ForMember(dest => dest.RoleDetails, opt => opt.MapFrom(src => src.RoleDetails))
                .ReverseMap();

            CreateMap<RoleDetail, RoleDetailDTO>().ReverseMap();
        }

    }
}
