using AutoMapper;
using Booking.API.Entities;
using Booking.API.Repositories.Interfaces;
using Booking.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Booking.API.Services
{
	public class BookingTourService : IBookingTourService
	{
		private readonly IBookingTourRepository _bookingTourRepository;
		private readonly ITourBookingRoomRepository _tourBookingRoomRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public BookingTourService(IBookingTourRepository bookingTourRepository, ITourBookingRoomRepository tourBookingRoomRepository, IMapper mapper, ILogger logger)
		{
			_bookingTourRepository = bookingTourRepository;
			_tourBookingRoomRepository = tourBookingRoomRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<ApiResponse<List<BookingTourResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: BookingTourService - GetAllAsync");

			try
			{
				var bookingTours = await _bookingTourRepository.GetBookingToursAsync();
				var data = _mapper.Map<List<BookingTourResponseDTO>>(bookingTours);

				_logger.Information("End: BookingTourService - GetAllAsync");
				return new ApiResponse<List<BookingTourResponseDTO>>(200, data, "Data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingTourService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<BookingTourResponseDTO>>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<BookingTourResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin: BookingTourService - GetByIdAsync: {id}");

			try
			{
				var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(id);

				if (bookingTour == null)
				{
					_logger.Warning($"Booking tour with ID {id} not found");
					return new ApiResponse<BookingTourResponseDTO>(404, null, "Booking tour not found");
				}

				var data = _mapper.Map<BookingTourResponseDTO>(bookingTour);

				_logger.Information($"End: BookingTourService - GetByIdAsync: {id}");
				return new ApiResponse<BookingTourResponseDTO>(200, data, "Booking tour data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingTourService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<BookingTourResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: BookingTourService - DeleteAsync: {id}");

			try
			{
				var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(id);

				if (bookingTour == null)
				{
					_logger.Warning($"Booking tour with ID {id} not found");
					return new ApiResponse<int>(404, 0, $"Booking tour with ID {id} not found");
				}

				bookingTour.DeletedAt = DateTime.UtcNow;
				var result = await _bookingTourRepository.UpdateBookingTourAsync(bookingTour);

				if (result <= 0)
				{
					_logger.Warning("Failed to delete booking tour");
					return new ApiResponse<int>(400, -1, "Failed to delete booking tour");
				}

				_logger.Information($"End: BookingTourService - DeleteAsync: {id} - Successfully deleted the booking tour.");
				return new ApiResponse<int>(200, 1, "Booking tour deleted successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingTourService - DeleteAsync: {ex.Message}", ex);
				return new ApiResponse<int>(500, 0, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<BookingTourResponseDTO>> CreateAsync(BookingTourRequestDTO item)
		{
			_logger.Information("Begin: BookingTourService - CreateAsync");

			try
			{
				var bookingTour = _mapper.Map<BookingTour>(item);
				bookingTour.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

				var result = await _bookingTourRepository.CreateBookingTourAsync(bookingTour);

				if (result > 0)
				{
					_logger.Information("End: BookingTourService - CreateAsync: Successfully created booking tour.");
					var data = _mapper.Map<BookingTourResponseDTO>(bookingTour);
					return new ApiResponse<BookingTourResponseDTO>(200, data, "Booking tour created successfully.");
				}
				else
				{
					_logger.Warning("End: BookingTourService - CreateAsync: Failed to create booking tour.");
					return new ApiResponse<BookingTourResponseDTO>(400, null, "Failed to create booking tour.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingTourService - CreateAsync: {ex.Message}", ex);
				return new ApiResponse<BookingTourResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<BookingTourResponseDTO>> UpdateAsync(int id, BookingTourRequestDTO item)
		{
			_logger.Information($"Begin: BookingTourService - UpdateAsync: {id}");

			try
			{
				var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(id);
				if (bookingTour == null)
				{
					_logger.Warning($"Booking tour with ID {id} not found.");
					return new ApiResponse<BookingTourResponseDTO>(404, null, $"Booking tour with ID {id} not found.");
				}

				_mapper.Map(item, bookingTour);
				bookingTour.UpdatedAt = DateTime.UtcNow;

				var result = await _bookingTourRepository.UpdateBookingTourAsync(bookingTour);

				if (result > 0)
				{
					_logger.Information($"End: BookingTourService - UpdateAsync: {id} - Successfully updated booking tour.");
					var data = _mapper.Map<BookingTourResponseDTO>(bookingTour);
					return new ApiResponse<BookingTourResponseDTO>(200, data, "Booking tour updated successfully.");
				}
				else
				{
					_logger.Warning($"End: BookingTourService - UpdateAsync: {id} - Failed to update booking tour.");
					return new ApiResponse<BookingTourResponseDTO>(400, null, "Failed to update booking tour.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingTourService - UpdateAsync: {ex.Message}", ex);
				return new ApiResponse<BookingTourResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}
	}
}
