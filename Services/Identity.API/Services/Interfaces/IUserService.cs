using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserResponseDTO>>> GetAllAsync();
        Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id);
		Task<ApiResponse<UserResponseDTO>> InsertAsync(UserRequestDTO item);
<<<<<<< HEAD
        Task<ApiResponse<UserResponseDTO>> UpdateAsync(int id, UpdateUserRequestDTO item);
=======
        Task<ApiResponse<UserResponseDTO>> UpdateAsync(UserRequestDTO item);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        Task<ApiResponse<int>> DeleteAsync(int id);
        Task<ApiResponse<string>> ChanageUserPasswordAsync(int userId,ChangeUserPasswordRequestDto dto);
    }
}
