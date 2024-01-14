using BookingApi.DTO;
using BookingApi.Helpers;

namespace BookingApi.Interfaces
{
    public interface ITourService
    {
        Task<List<TourResponseDTO>> GetAll();
        Task<TourResponseDTO> GetById(int id);
        Task<List<ScheduleResponseDTO>> GetSchedulesByTourId(int id);
        Task<APIResponse<int>> Insert(TourRequestDTO item);
        Task<TourResponseDTO> Update(TourRequestDTO item);
        Task<APIResponse<int>> Delete(int id);
    }
}
