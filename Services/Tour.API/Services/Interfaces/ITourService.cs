using Shared.DTOs;
using Shared.Helper;

namespace Tour.API.Services.Interfaces
{
    public interface ITourService
    {
        Task<ApiResponse<List<TourResponseDTO>>> GetAllAsync();
        Task<ApiResponse<TourResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<int>> CreateAsync(TourRequestDTO item);
        Task<ApiResponse<TourResponseDTO>> UpdateAsync(TourRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
        Task<ApiResponse<List<TourResponseDTO>>> SearchToursAsync(TourSearchRequestDTO searchRequest);
        Task<ApiResponse<List<TourResponseDTO>>> SearchToursWithPaginationAsync(TourSearchRequestDTO searchRequest);
    }
}
