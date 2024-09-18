using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionResponseDTO>> GetAll();
        Task<PermissionResponseDTO> GetById(int id);
        Task<ApiResponse<int>> Insert(PermissionRequestDTO item);
        Task<PermissionResponseDTO> Update(PermissionRequestDTO item);
        Task<ApiResponse<int>> Delete(int id);
    }
}
