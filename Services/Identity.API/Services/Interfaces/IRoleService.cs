using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleResponseDTO>> GetAll();
        Task<RoleResponseDTO> GetById(int id);
        Task<List<RoleDetailDTO>> GetRoleDetailAllByRoleId(int id);
        Task<RoleDetailDTO> UpdateRoleDetailByRoleId(RoleDetailDTO item);
        Task<ApiResponse<int>> Insert(RoleRequestDTO item);
        Task<RoleResponseDTO> Update(RoleRequestDTO item);
        Task<ApiResponse<int>> Delete(int id);
    }
}
