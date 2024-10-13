using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Services.Interfaces
{
    public interface IBookingRoomService
    {
        Task<ApiResponse<List<BookingRoomResponseDTO>>> GetAllAsync();
        Task<ApiResponse<BookingRoomResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<BookingRoomResponseDTO>> GetByUserIdAsync(string name);
        Task<ApiResponse<BookingRoomResponseDTO>> UpdateAsync(HotelRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
    }
}
