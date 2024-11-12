using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Services.Interfaces
{
	public interface IBookingTourService
    {
		Task<ApiResponse<List<BookingTourCustomResponseDTO>>> GetAllAsync();
		Task<ApiResponse<BookingTourCustomResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<List<BookingTourCustomResponseDTO>>> GetCurrentUserAsync(int userId);
        Task<ApiResponse<BookingTourCustomResponseDTO>> UpdateBookingTourInfoAsync(int bookingTourId, UpdateBookingTourInfoRequest request, int userId, int role);
        Task<ApiResponse<string>> DeleteBookingTourAsync(int bookingTourId, int userId);
        Task<ApiResponse<string>> UpdateStatusBookingTourAsync(int bookingTourId, UpdateBookingStatusDTO dto);

    }
}
