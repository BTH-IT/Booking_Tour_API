using BookingApi.DTO;
using BookingApi.Helpers;

namespace BookingApi.Services.Interfaces
{
    public interface IBookingTourService
    {
        Task<List<BookingTourResponseDTO>> GetAll();
        Task<BookingTourResponseDTO> GetById(int id);
        Task<APIResponse<int>> Insert(BookingTourRequestDTO item);
        Task<BookingTourResponseDTO> Update(BookingTourRequestDTO item);
        Task<APIResponse<int>> Delete(int id);
    }
}
