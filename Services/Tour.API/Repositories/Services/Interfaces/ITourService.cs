using Shared.DTOs;
using Shared.Helper;

namespace Tour.API.Services.Interfaces
{
    public interface ITourService
    {
        Task<ApiResponse<List<TourResponseDTO>>> GetAllAsync();
        Task<ApiResponse<TourResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<TourResponseDTO>> CreateAsync(TourRequestDTO item);
        Task<ApiResponse<TourResponseDTO>> UpdateAsync(int id, TourRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
        Task<ApiResponse<TourSearchResponseDTO>> SearchToursAsync(TourSearchRequestDTO searchRequest);
    }
}
