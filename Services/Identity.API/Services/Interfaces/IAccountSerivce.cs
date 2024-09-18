using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services.Interfaces
{
    public interface IAccountSerivce
    {
        Task<ApiResponse<List<AccountResponseDTO>>> GetAllAsync();
        Task<ApiResponse<AccountResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<int>> CreateAsync(AccountRequestDTO item);
        Task<ApiResponse<AccountResponseDTO>> UpdateAsync(AccountRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);

    }
}
