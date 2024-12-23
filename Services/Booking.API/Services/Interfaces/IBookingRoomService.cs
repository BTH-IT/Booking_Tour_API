﻿using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Services.Interfaces
{
	public interface IBookingRoomService
	{
		Task<ApiResponse<List<BookingRoomResponseDTO>>> GetAllAsync();
		Task<ApiResponse<BookingRoomResponseDTO>> GetByIdAsync(int id);
		Task<ApiResponse<List<BookingRoomResponseDTO>>> GetCurrentUserAsync(int userId);
		Task<ApiResponse<RoomBookingDataDTO>> GetRoomCheckInCheckOutDataAsync(int roomId);
		Task<ApiResponse<string>> DeleteBookingRoomIdAsync(int bookingRoomId,int userId);
		Task<ApiResponse<string>> UpdateStatusBookingRoomAsync(int bookingRoomId, UpdateBookingStatusDTO dto);
    }
}
