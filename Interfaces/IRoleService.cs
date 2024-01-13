using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Models;

namespace BookingApi.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleResponseDTO>> GetAll();
        Task<RoleResponseDTO> GetById(int id);
        Task<List<RoleDetailDTO>> GetRoleDetailAllByRoleId(int id);
        Task<RoleDetailDTO> UpdateRoleDetailByRoleId(RoleDetailDTO item);
        Task<APIResponse<int>> Insert(RoleRequestDTO item);
        Task<RoleResponseDTO> Update(RoleRequestDTO item);
        Task<APIResponse<int>> Delete(int id);
    }
}
