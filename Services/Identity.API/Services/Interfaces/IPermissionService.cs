using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<ApiResponse<List<PermissionResponseDTO>>> GetAllAsync();
        Task<ApiResponse<PermissionResponseDTO>> GetByIdAsync(int id);
		Task<ApiResponse<PermissionResponseDTO>> InsertAsync(PermissionRequestDTO item);
        Task<ApiResponse<PermissionResponseDTO>> UpdateAsync(PermissionRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
    }
}
