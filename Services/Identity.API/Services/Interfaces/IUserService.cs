using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserResponseDTO>>> GetAllAsync();
        Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id);
        Task<ApiResponse<int>> InsertAsync(UserRequestDTO item);
        Task<ApiResponse<UserResponseDTO>> UpdateAsync(UserRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
    }
}
