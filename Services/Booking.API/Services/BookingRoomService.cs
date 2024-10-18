using AutoMapper;
using Booking.API.Entities;
using Booking.API.GrpcClient.Protos;
using Booking.API.Repositories.Interfaces;
using Booking.API.Services.Interfaces;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Booking.API.Services
{
	public class BookingRoomService : IBookingRoomService
	{
		private readonly IBookingRoomRepository _bookingRoomRepository;
		private readonly IDetailBookingRoomRepository _detailBookingRoomRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public BookingRoomService(IBookingRoomRepository bookingRoomRepository, IDetailBookingRoomRepository detailBookingRoomRepository, IMapper mapper, ILogger logger)
		{
			_bookingRoomRepository = bookingRoomRepository;
			_detailBookingRoomRepository = detailBookingRoomRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<ApiResponse<List<BookingRoomResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: BookingRoomService - GetAllAsync");

			try
			{
				var bookingRooms = await _bookingRoomRepository.GetBookingRoomsAsync();

				var data = _mapper.Map<List<BookingRoomResponseDTO>>(bookingRooms);

				_logger.Information("End: BookingRoomService - GetAllAsync");
				return new ApiResponse<List<BookingRoomResponseDTO>>(200, data, "Data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingRoomService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<BookingRoomResponseDTO>>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<BookingRoomResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin: BookingRoomService - GetByIdAsync: {id}");

			try
			{
				var bookingRoom = await _bookingRoomRepository.GetBookingRoomByIdAsync(id);

				if (bookingRoom == null)
				{
					_logger.Warning($"Booking room with ID {id} not found");
					return new ApiResponse<BookingRoomResponseDTO>(404, null, "Booking room not found");
				}

				var data = _mapper.Map<BookingRoomResponseDTO>(bookingRoom);

				_logger.Information($"End: BookingRoomService - GetByIdAsync: {id}");
				return new ApiResponse<BookingRoomResponseDTO>(200, data, "Booking room data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingRoomService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<BookingRoomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<BookingRoomResponseDTO>> CreateAsync(BookingRoomRequestDTO item)
		{
			_logger.Information("Begin: BookingRoomService - CreateAsync");

			try
			{
				var bookingRoom = _mapper.Map<BookingRoom>(item);
				bookingRoom.CreatedAt = DateTime.UtcNow;
				
				var result = await _bookingRoomRepository.CreateBookingRoomAsync(bookingRoom);

				if (result > 0)
				{
					_logger.Information("End: BookingRoomService - CreateAsync: Successfully created booking room.");
					var updatedBookingRoom = await _bookingRoomRepository.GetBookingRoomByIdAsync(result);
					var data = _mapper.Map<BookingRoomResponseDTO>(updatedBookingRoom);
					return new ApiResponse<BookingRoomResponseDTO>(200, data, "Booking room created successfully.");
				}
				else
				{
					_logger.Warning("End: BookingRoomService - CreateAsync: Failed to create booking room.");
					return new ApiResponse<BookingRoomResponseDTO>(400, null, "Failed to create booking room.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingRoomService - CreateAsync: {ex.Message}", ex);
				return new ApiResponse<BookingRoomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<BookingRoomResponseDTO>> UpdateAsync(int id, BookingRoomRequestDTO item)
		{
			_logger.Information($"Begin: BookingRoomService - UpdateAsync: {id}");

			try
			{
				var bookingRoom = await _bookingRoomRepository.GetBookingRoomByIdAsync(id);
				if (bookingRoom == null)
				{
					_logger.Warning($"Booking room with ID {id} not found.");
					return new ApiResponse<BookingRoomResponseDTO>(404, null, $"Booking room with ID {id} not found.");
				}
				_mapper.Map(item, bookingRoom);
				bookingRoom.UpdatedAt = DateTime.UtcNow;

				var result = await _bookingRoomRepository.UpdateBookingRoomAsync(bookingRoom);

				if (result > 0)
				{
					_logger.Information($"End: BookingRoomService - UpdateAsync: {id} - Successfully updated booking room.");
					var updatedBookingRoom = await _bookingRoomRepository.GetBookingRoomByIdAsync(id);
					var data = _mapper.Map<BookingRoomResponseDTO>(updatedBookingRoom);
					return new ApiResponse<BookingRoomResponseDTO>(200, data, "Booking room updated successfully.");
				}
				else
				{
					_logger.Warning($"End: BookingRoomService - UpdateAsync: {id} - Failed to update booking room.");
					return new ApiResponse<BookingRoomResponseDTO>(400, null, "Failed to update booking room.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingRoomService - UpdateAsync: {ex.Message}", ex);
				return new ApiResponse<BookingRoomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}
		
		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: BookingRoomService - DeleteAsync: {id}");

			try
			{
				var bookingRoom = await _bookingRoomRepository.GetBookingRoomByIdAsync(id);

				if (bookingRoom == null)
				{
					_logger.Warning($"Booking room with ID {id} not found");
					return new ApiResponse<int>(404, 0, $"Booking room with ID {id} not found");
				}

				if (bookingRoom.CheckOut.HasValue && DateTime.UtcNow <= bookingRoom.CheckOut.Value)
				{
					_logger.Warning("The booking room is still occupied");
					return new ApiResponse<int>(400, 0, "The booking room is still occupied");
				}

				var detailBookingRoom = await _detailBookingRoomRepository.GetDetailBookingRoomByBookingRoomIdAsync(id);
				detailBookingRoom.DeletedAt = DateTime.UtcNow;

				var resultDetail = await _detailBookingRoomRepository.UpdateDetailBookingRoomAsync(detailBookingRoom);
				if (resultDetail <= 0)
				{
					_logger.Warning("Failed to delete detail booking room");
					return new ApiResponse<int>(400, -1, "An error occurred while deleting the detail booking room");
				}

				bookingRoom.DeletedAt = DateTime.UtcNow;
				var result = await _bookingRoomRepository.UpdateBookingRoomAsync(bookingRoom);

				if (result <= 0)
				{
					_logger.Warning("Failed to delete booking room");
					return new ApiResponse<int>(400, -1, "An error occurred while deleting the booking room");
				}

				_logger.Information($"End: BookingRoomService - DeleteAsync: {id} - Successfully deleted the booking room.");
				return new ApiResponse<int>(200, 1, "Booking room deleted successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingRoomService - DeleteAsync: {ex.Message}", ex);
				return new ApiResponse<int>(500, 0, $"An error occurred: {ex.Message}");
			}
		}
	}
}
