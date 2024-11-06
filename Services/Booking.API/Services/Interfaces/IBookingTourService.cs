using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Services.Interfaces
{
	public interface IBookingTourService
    {
		Task<ApiResponse<List<BookingTourResponseDTO>>> GetAllAsync();
		Task<ApiResponse<BookingTourResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<List<BookingTourResponseDTO>>> GetCurrentUserAsync(int userId);
        Task<ApiResponse<BookingTourResponseDTO>> UpdateBookingTourInfoAsync(int bookingTourId, UpdateBookingTourInfoRequest request, int userId, int role);
    }
}
