using Shared.DTOs;
using Shared.Helper;

namespace Tour.API.Services.Interfaces
{
    public interface IDestinationService
    {
        Task<ApiResponse<List<DestinationResponseDTO>>> GetAllAsync();
        Task<ApiResponse<DestinationResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<DestinationResponseDTO>> CreateAsync(DestinationRequestDTO item);
        Task<ApiResponse<DestinationResponseDTO>> UpdateAsync(int id, DestinationRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
    }
}
