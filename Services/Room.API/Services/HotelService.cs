using AutoMapper;
<<<<<<< HEAD
using MassTransit;
using Microsoft.EntityFrameworkCore;
=======
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
<<<<<<< HEAD
using EventBus.IntergrationEvents.Events;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using ILogger = Serilog.ILogger;

namespace Room.API.Services
{
	public class HotelService : IHotelService
	{
		private readonly IHotelRepository _hotelRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
<<<<<<< HEAD
		private readonly IPublishEndpoint _publishEndpoint;

		public HotelService(IHotelRepository hotelRepository, IRoomRepository roomRepository, IMapper mapper, ILogger logger, IPublishEndpoint publishEndpoint)
=======

		public HotelService(IHotelRepository hotelRepository, IRoomRepository roomRepository, IMapper mapper, ILogger logger)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			_hotelRepository = hotelRepository;
			_roomRepository = roomRepository;
			_mapper = mapper;
			_logger = logger;
<<<<<<< HEAD
			_publishEndpoint = publishEndpoint;
		}

		public async Task<ApiResponse<List<HotelResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: HotelService - GetAllAsync");

			try
			{
				var hotels = await _hotelRepository.GetHotelsAsync();

				if (!hotels?.Any() ?? true)
				{
					_logger.Warning("No hotels");
					return new ApiResponse<List<HotelResponseDTO>>(204, null, "No hotels");
				}

				var data = _mapper.Map<List<HotelResponseDTO>>(hotels);

				_logger.Information("End: HotelService - GetAllAsync");
				return new ApiResponse<List<HotelResponseDTO>>(200, data, "Data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in HotelService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<HotelResponseDTO>>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information("Begin: HotelService - GetByIdAsync");

			try
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(id);
				if (hotel == null)
				{
					_logger.Warning($"Hotel with id {id} not found or deleted");
					return new ApiResponse<HotelResponseDTO>(404, null, $"Hotel with id {id} not found or deleted");
				}

				var data = _mapper.Map<HotelResponseDTO>(hotel);

				_logger.Information("End: HotelService - GetByIdAsync");
				return new ApiResponse<HotelResponseDTO>(200, data, "Hotel data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in HotelService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<HotelResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information("Begin: HotelService - GetByNameAsync");

			try
			{
				var hotel = await _hotelRepository.GetHotelByNameAsync(name);

				if (hotel == null || hotel.DeletedAt != null)
				{
					_logger.Warning($"Hotel with name {name} not found or deleted");
					return new ApiResponse<HotelResponseDTO>(404, null, $"Hotel with name {name} not found or deleted");
				}

				var data = _mapper.Map<HotelResponseDTO>(hotel);

				_logger.Information("End: HotelService - GetByNameAsync");
				return new ApiResponse<HotelResponseDTO>(200, data, "Hotel data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in HotelService - GetByNameAsync: {ex.Message}", ex);
				return new ApiResponse<HotelResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		}

		public async Task<ApiResponse<HotelResponseDTO>> CreateAsync(HotelRequestDTO item)
		{
			_logger.Information("Begin: HotelService - CreateAsync");

<<<<<<< HEAD
			try
			{
				var existingHotel = await _hotelRepository.GetHotelByNameAsync(item.Name);
				if (existingHotel != null)
				{
					_logger.Warning($"Hotel with name {item.Name} already exists");
					return new ApiResponse<HotelResponseDTO>(400, null, $"Hotel with name {item.Name} already exists");
				}

				var hotelEntity = _mapper.Map<Hotel>(item);
				hotelEntity.CreatedAt = DateTime.UtcNow;

				var newId = await _hotelRepository.CreateHotelAsync(hotelEntity);

				if (newId <= 0)
				{
					_logger.Warning("Failed to create hotel");
					return new ApiResponse<HotelResponseDTO>(400, null, "Error occurred while creating the hotel");
				}

				var createdHotel = await _hotelRepository.GetHotelByIdAsync(newId);
				var responseData = _mapper.Map<HotelResponseDTO>(createdHotel);
				await _publishEndpoint.Publish(new HotelEvent
				{
					Id = Guid.NewGuid(),
					Data = responseData,
					Type = "CREATE"
				});

				_logger.Information("End: HotelService - CreateAsync");
				return new ApiResponse<HotelResponseDTO>(200, responseData, "Hotel created successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in HotelService - CreateAsync: {ex.Message}", ex);
				return new ApiResponse<HotelResponseDTO>(500, null, $"An error occurred while creating the hotel: {ex.Message}");
			}
		}

		public async Task<ApiResponse<HotelResponseDTO>> UpdateAsync(int id, HotelRequestDTO item)
		{
			_logger.Information("Begin: HotelService - UpdateAsync");

			try
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(id);

				if (hotel == null || hotel.DeletedAt != null)
				{
					_logger.Warning($"Hotel with ID {id} not found or deleted");
					return new ApiResponse<HotelResponseDTO>(404, null, $"Hotel with ID {id} not found or deleted");
				}

				if (await _hotelRepository.FindByCondition(h => h.Name.Equals(item.Name) && h.Id != id && h.DeletedAt == null).FirstOrDefaultAsync() != null)
				{
					_logger.Warning($"Hotel name '{item.Name}' already exists");
					return new ApiResponse<HotelResponseDTO>(400, null, $"Hotel name '{item.Name}' already exists");
				}

				_mapper.Map(item, hotel);
				hotel.UpdatedAt = DateTime.UtcNow;

				var result = await _hotelRepository.UpdateAsync(hotel);

				if (result <= 0)
				{
					_logger.Warning("Failed to update hotel");
					return new ApiResponse<HotelResponseDTO>(400, null, "Error occurred while updating hotel");
				}

				var updatedHotel = await _hotelRepository.GetHotelByIdAsync(id);
				var responseData = _mapper.Map<HotelResponseDTO>(updatedHotel);
				await _publishEndpoint.Publish(new HotelEvent
				{
					Id = Guid.NewGuid(),
					Data = responseData,
					Type = "UPDATE"
				});

				_logger.Information("End: HotelService - UpdateAsync");
				return new ApiResponse<HotelResponseDTO>(200, responseData, "Hotel updated successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in HotelService - UpdateAsync: {ex.Message}", ex);
				return new ApiResponse<HotelResponseDTO>(500, null, $"An error occurred while updating the hotel: {ex.Message}");
			}
=======
			var existingHotel = await _hotelRepository.GetHotelByNameAsync(item.Name);
			if (existingHotel != null)
			{
				return new ApiResponse<HotelResponseDTO>(400, null, "Hotel already exists");
			}

			var hotelEntity = _mapper.Map<Hotel>(item);
			hotelEntity.CreatedAt = DateTime.UtcNow;
			var newId = await _hotelRepository.CreateAsync(hotelEntity);
			var createdHotel = await _hotelRepository.GetHotelByIdAsync(newId);
			var responseData = _mapper.Map<HotelResponseDTO>(createdHotel);

			_logger.Information("End: HotelService - CreateAsync");
			return new ApiResponse<HotelResponseDTO>(200, responseData, "Hotel created successfully");
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: HotelService - DeleteAsync : {id}");

			using var transaction = await _hotelRepository.BeginTransactionAsync();
			try
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(id);
				if (hotel == null)
				{
<<<<<<< HEAD
					_logger.Warning($"Hotel with ID {id} not found or deleted");
					return new ApiResponse<int>(404, 0, $"Hotel with ID {id} not found or deleted");
				}

				var rooms = await _roomRepository.GetRoomByHotelIdAsync(id);
				if (rooms.Any(r => r.IsAvailable))
				{
					_logger.Warning($"Rooms {string.Join(", ", rooms.Where(r => r.IsAvailable).Select(r => r.Id))} are currently booked and cannot be deleted.");
=======
					return new ApiResponse<int>(404, 0, "Hotel not found.");
				}

				hotel.DeletedAt = DateTime.UtcNow;
				var rooms = await _roomRepository.FindByCondition(r => r.HotelId == id).ToListAsync();
				if (rooms.Any(r => r.IsAvailable))
				{
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
					return new ApiResponse<int>(400, 0, $"Rooms {string.Join(", ", rooms.Where(r => r.IsAvailable).Select(r => r.Id))} are currently booked and cannot be deleted.");
				}

				foreach (var room in rooms)
				{
					room.DeletedAt = DateTime.UtcNow;
					await _roomRepository.UpdateRoomAsync(room);
				}

<<<<<<< HEAD
				hotel.DeletedAt = DateTime.UtcNow;
				var result = await _hotelRepository.UpdateHotelAsync(hotel);
				await transaction.CommitAsync();

				if (result <= 0)
				{
					_logger.Warning("Failed to delete hotel");
					return new ApiResponse<int>(400, -1, "Error occurred while deleting hotel");
				}

				var responseData = _mapper.Map<HotelResponseDTO>(hotel);

				await _publishEndpoint.Publish(new HotelEvent
				{
					Id = Guid.NewGuid(),
					Data = responseData,
					Type = "DELETE"
				});

				_logger.Information($"End: HotelService - DeleteAsync : {id} - Successfully soft deleted the hotel and its rooms.");
				return new ApiResponse<int>(200, rooms.Count(), "Successfully soft deleted the hotel and its associated rooms.");
=======
				await _hotelRepository.UpdateAsync(hotel);
				await transaction.CommitAsync();

				_logger.Information($"End: HotelService - DeleteAsync : {id} - Successfully soft deleted the hotel and its rooms.");
				return new ApiResponse<int>(200, rooms.Count, "Successfully soft deleted the hotel and its associated rooms.");
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.Error($"Error during hotel deletion: {ex.Message}");
<<<<<<< HEAD
				return new ApiResponse<int>(500, 0, $"An error occurred while deleting the hotel: {ex.Message}");
			}
		}
	}
}
=======
				return new ApiResponse<int>(500, 0, "An error occurred while deleting the hotel.");
			}
		}

		public async Task<ApiResponse<List<HotelResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: HotelService - GetAllAsync");

			var hotels = await _hotelRepository.FindAll(false, hotel => hotel.Rooms)
												.Where(h => h.DeletedAt == null)
												.ToListAsync();

			var data = _mapper.Map<List<HotelResponseDTO>>(hotels);

			_logger.Information("End: HotelService - GetAllAsync");
			return new ApiResponse<List<HotelResponseDTO>>(200, data, "Data retrieved successfully");
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information("Begin: HotelService - GetByIdAsync");
			var hotel = await _hotelRepository.GetHotelByIdAsync(id);

			if (hotel == null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Hotel not found");
			}
			var data = _mapper.Map<HotelResponseDTO>(hotel);

			_logger.Information("End: HotelService - GetByIdAsync");
			return new ApiResponse<HotelResponseDTO>(200, data, "Hotel data retrieved successfully");
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information("Begin: HotelService - GetByNameAsync");
			var hotel = await _hotelRepository.GetHotelByNameAsync(name);

			if (hotel == null || hotel.DeletedAt != null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Hotel not found");
			}
			var data = _mapper.Map<HotelResponseDTO>(hotel);
			_logger.Information("End: HotelService - GetByNameAsync");
			return new ApiResponse<HotelResponseDTO>(200, data, "Hotel data retrieved successfully");
		}

		public async Task<ApiResponse<HotelResponseDTO>> UpdateAsync(int id, HotelRequestDTO item)
		{
			_logger.Information("Begin: HotelService - UpdateAsync");

			var hotel = await _hotelRepository.GetHotelByIdAsync(id);

			if (hotel == null || hotel.DeletedAt != null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Hotel not found");
			}

			if (await _hotelRepository.FindByCondition(h => h.Name.Equals(item.Name) && h.Id != id && h.DeletedAt == null).FirstOrDefaultAsync() != null)
			{
				return new ApiResponse<HotelResponseDTO>(400, null, "Hotel name already exists");
			}

			var existingReviews = hotel.ReviewList;

			hotel = _mapper.Map<Hotel>(item);
			hotel.Id = id;

			if (existingReviews != null)
			{
				hotel.ReviewList = existingReviews;
			}

			hotel.UpdatedAt = DateTime.UtcNow;
			var result = await _hotelRepository.UpdateAsync(hotel);

			if (result > 0)
			{
				var updatedHotel = await _hotelRepository.GetHotelByIdAsync(id);
				var responseData = _mapper.Map<HotelResponseDTO>(updatedHotel);
				_logger.Information("End: HotelService - UpdateAsync");
				return new ApiResponse<HotelResponseDTO>(200, responseData, "Hotel updated successfully");
			}

			_logger.Information("End: HotelService - UpdateAsync");
			return new ApiResponse<HotelResponseDTO>(400, null, "Error occurred while updating hotel");
		}
	}
}
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
