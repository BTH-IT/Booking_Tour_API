using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<List<RoleResponseDTO>>> GetAllAsync();
        Task<ApiResponse<RoleResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<RoleDetailDTO>> UpdateRoleDetailByRoleIdAsync(string roleId,RoleDetailDTO item);
		Task<ApiResponse<RoleResponseDTO>> InsertAsync(RoleRequestDTO item);
        Task<ApiResponse<RoleResponseDTO>> UpdateAsync(RoleRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
    }
}
