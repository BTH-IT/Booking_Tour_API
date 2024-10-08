using Identity.API.Repositories.Interfaces;
using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IAuthService
    {
		Task<ApiResponse<UserResponseDTO>> RegisterAsync(AuthRegisterDTO registerDTO);
        Task<ApiResponse<AuthResponseDTO>> LoginAsync(AuthLoginDTO loginDTO);
        Task<ApiResponse<string>> RefreshToken(string refreshToken);
    }
}
