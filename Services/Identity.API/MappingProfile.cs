using AutoMapper;
using Identity.API.Entites;
using Shared.DTOs;

namespace Identity.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserResponseDTO>().ReverseMap();
            CreateMap<UserRequestDTO, User>();
            
            CreateMap<Role, RoleResponseDTO>().ReverseMap(); 
            CreateMap<RoleDetail,RoleDetailDTO>().ReverseMap();

            CreateMap<RoleRequestDTO, Role>();
            
            CreateMap<Account,AccountResponseDTO>().ReverseMap();   
            CreateMap<AccountRequestDTO, Account>();

            CreateMap<Permission, PermissionResponseDTO>().ReverseMap();
            CreateMap<PermissionRequestDTO, Permission>();   
        }
    }   
}
