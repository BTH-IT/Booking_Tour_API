using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDTO>> GetAllAsync();
        Task<UserResponseDTO> GetUserByIdAsync(int id);
        Task<ApiResponse<int>> InsertAsync(UserRequestDTO item);
        Task<UserResponseDTO> UpdateAsync(UserRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
    }
}
