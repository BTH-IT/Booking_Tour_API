using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Services.Interfaces
{
	public interface IBookingTourService
    {
		Task<ApiResponse<List<BookingTourResponseDTO>>> GetAllAsync();
		Task<ApiResponse<BookingTourResponseDTO>> GetByIdAsync(int id);
		Task<ApiResponse<BookingTourResponseDTO>> CreateAsync(BookingTourRequestDTO item);
		Task<ApiResponse<BookingTourResponseDTO>> UpdateAsync(int id, BookingTourRequestDTO item);
		Task<ApiResponse<int>> DeleteAsync(int id);
	}
}
