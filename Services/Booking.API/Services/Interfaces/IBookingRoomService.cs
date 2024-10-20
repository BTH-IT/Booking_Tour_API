using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Services.Interfaces
{
	public interface IBookingRoomService
	{
		Task<ApiResponse<List<BookingRoomResponseDTO>>> GetAllAsync();
		Task<ApiResponse<BookingRoomResponseDTO>> GetByIdAsync(int id);
		Task<ApiResponse<BookingRoomResponseDTO>> CreateAsync(BookingRoomRequestDTO item);
		Task<ApiResponse<BookingRoomResponseDTO>> UpdateAsync(int id, BookingRoomRequestDTO item);
		Task<ApiResponse<int>> DeleteAsync(int id);
		Task<ApiResponse<List<BookingRoomResponseDTO>>> GetCurrentUserAsync(int userId);
	}
}
