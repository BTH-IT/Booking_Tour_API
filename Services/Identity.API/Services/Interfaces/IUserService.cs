using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserResponseDTO>>> GetAllAsync();
        Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id);
		Task<ApiResponse<UserResponseDTO>> InsertAsync(UserRequestDTO item);
        Task<ApiResponse<UserResponseDTO>> UpdateAsync(int id, UpdateUserRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
        Task<ApiResponse<string>> ChanageUserPasswordAsync(int userId,ChangeUserPasswordRequestDto dto);
    }
}
