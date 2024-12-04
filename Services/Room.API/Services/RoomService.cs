using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;
using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Room.API.Services
{
	public class RoomService : IRoomService
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IHotelRepository _hotelRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IDistributedCache _cache;

		public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper, ILogger logger, IPublishEndpoint publishEndpoint, IDistributedCache cache)
		{
			_roomRepository = roomRepository;
			_hotelRepository = hotelRepository;
			_mapper = mapper;
			_logger = logger;
			_publishEndpoint = publishEndpoint;
			_cache = cache;
		}

		public async Task<ApiResponse<List<RoomResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: RoomService - GetAllAsync");
			try
			{
				var cacheKey = "Room_All";
				var cachedData = await _cache.GetStringAsync(cacheKey);
				if (!string.IsNullOrEmpty(cachedData))
				{
					var cachedRooms = JsonSerializer.Deserialize<List<RoomResponseDTO>>(cachedData);
					_logger.Information("End: RoomService - GetAllAsync");
					return new ApiResponse<List<RoomResponseDTO>>(200, cachedRooms, "Data retrieved successfully", true);
				}

				var rooms = await _roomRepository.GetRoomsAsync();

				if (!rooms?.Any() ?? true)
				{
					_logger.Warning("No rooms");
					return new ApiResponse<List<RoomResponseDTO>>(204, null, "No rooms");
				}

				var data = _mapper.Map<List<RoomResponseDTO>>(rooms);

				// Cache the data
				var cacheOptions = new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
				};
				await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), cacheOptions);

				_logger.Information("End: RoomService - GetAllAsync");
				return new ApiResponse<List<RoomResponseDTO>>(200, data, "Data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in RoomService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<RoomResponseDTO>>(500, null, $"An error occurred {ex.Message}");
			}
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information("Begin: RoomService - GetByIdAsync");

			try
			{
				var cacheKey = $"Room_{id}";
				var cachedData = await _cache.GetStringAsync(cacheKey);
				if (!string.IsNullOrEmpty(cachedData))
				{
					var cachedRoom = JsonSerializer.Deserialize<RoomResponseDTO>(cachedData);

					_logger.Information("End: RoomService - GetByIdAsync");
					return new ApiResponse<RoomResponseDTO>(200, cachedRoom, "Room data retrieved successfully", true);
				}

				var room = await _roomRepository.GetRoomByIdAsync(id);
				if (room == null || room.DeletedAt != null)
				{
					_logger.Warning($"Room with id {id} not found or deleted");
					return new ApiResponse<RoomResponseDTO>(404, null, $"Room with id {id} not found or deleted");
				}

				var data = _mapper.Map<RoomResponseDTO>(room);

				// Cache the data
				var cacheOptions = new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
				};
				await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), cacheOptions);

				_logger.Information("End: RoomService - GetByIdAsync");
				return new ApiResponse<RoomResponseDTO>(200, data, "Room data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in RoomService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<RoomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information("Begin: RoomService - GetByNameAsync");

			try
			{
				var cacheKey = $"Room_{name}";
				var cachedData = await _cache.GetStringAsync(cacheKey);
				if (!string.IsNullOrEmpty(cachedData))
				{
					var cachedRoom = JsonSerializer.Deserialize<RoomResponseDTO>(cachedData);

					_logger.Information("End: RoomService - GetByNameAsync");
					return new ApiResponse<RoomResponseDTO>(200, cachedRoom, "Room data retrieved successfully", true);
				}

				var room = await _roomRepository.GetRoomByNameAsync(name);
				if (room == null || room.DeletedAt != null)
				{
					_logger.Warning($"Room with name {name} not found or deleted");
					return new ApiResponse<RoomResponseDTO>(404, null, $"Room with name {name} not found or deleted");
				}

				var data = _mapper.Map<RoomResponseDTO>(room);

				// Cache the data
				var cacheOptions = new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
				};
				await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), cacheOptions);

				_logger.Information("End: RoomService - GetByNameAsync");
				return new ApiResponse<RoomResponseDTO>(200, data, "Room data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in RoomService - GetByNameAsync: {ex.Message}", ex);
				return new ApiResponse<RoomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<RoomResponseDTO>> CreateAsync(RoomRequestDTO item)
		{
			_logger.Information("Begin: RoomService - CreateAsync");

			try
			{
				var existingRoom = await _roomRepository.GetRoomByNameAsync(item.Name);
				if (existingRoom != null)
				{
					_logger.Warning($"Room with name {item.Name} already exists");
					return new ApiResponse<RoomResponseDTO>(400, null, $"Room with name {item.Name} already exists");
				}

				var roomEntity = _mapper.Map<RoomEntity>(item);
				roomEntity.CreatedAt = DateTime.UtcNow;
				var newId = await _roomRepository.CreateRoomAsync(roomEntity);

				if (newId <= 0)
				{
					_logger.Warning("Failed to create Room");
					return new ApiResponse<RoomResponseDTO>(400, null, "Error occurred while creating the Room");
				}

				var createdRoom = await _roomRepository.GetRoomByIdAsync(newId);
				var data = _mapper.Map<RoomResponseDTO>(createdRoom);
				await _publishEndpoint.Publish(new RoomEvent
				{
					Id = Guid.NewGuid(),
					Data = data,
					Type = "CREATE"
				});

				// Invalidate cache
				await _cache.RemoveAsync("Room_All");

				_logger.Information("End: RoomService - CreateAsync");
				return new ApiResponse<RoomResponseDTO>(200, data, "Room created successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in RoomService - CreateAsync: {ex.Message}", ex);
				return new ApiResponse<RoomResponseDTO>(500, null, $"An error occurred while creating the room: {ex.Message}");
			}
		}

		public async Task<ApiResponse<RoomResponseDTO>> UpdateAsync(int id, RoomRequestDTO item)
		{
			_logger.Information("Begin: RoomService - UpdateAsync");

			try
			{
				var room = await _roomRepository.GetRoomByIdAsync(id);
				if (room == null || room.DeletedAt != null)
				{
					_logger.Warning($"Room with ID {id} not found or deleted");
					return new ApiResponse<RoomResponseDTO>(404, null, $"Room with ID {id} not found or deleted");
				}

				if (await _roomRepository.FindByCondition(r => r.Name.Equals(item.Name) && r.Id != id && r.DeletedAt == null).FirstOrDefaultAsync() != null)
				{
					_logger.Warning($"Room name {item.Name} already exists");
					return new ApiResponse<RoomResponseDTO>(400, null, $"Room name {item.Name} already exists");
				}

				_mapper.Map(item, room);
				room.UpdatedAt = DateTime.UtcNow;

				var result = await _roomRepository.UpdateRoomAsync(room);
				if (result <= 0)
				{
					_logger.Warning("End: RoomService - UpdateAsync - Update failed");
					return new ApiResponse<RoomResponseDTO>(400, null, "Error occurred while updating room");
				}

				var updatedRoom = await _roomRepository.GetRoomByIdAsync(id);
				var data = _mapper.Map<RoomResponseDTO>(updatedRoom);
				await _publishEndpoint.Publish(new RoomEvent
				{
					Id = Guid.NewGuid(),
					Data = data,
					Type = "UPDATE"
				});

				// Invalidate cache
				await _cache.RemoveAsync($"Room_{id}");
				await _cache.RemoveAsync($"Room_{room.Name}");
				await _cache.RemoveAsync("Room_All");

				_logger.Information("End: RoomService - UpdateAsync");
				return new ApiResponse<RoomResponseDTO>(200, data, "Room updated successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Unexpected error in RoomService - UpdateAsync: {ex.Message}", ex);
				return new ApiResponse<RoomResponseDTO>(500, null, $"An error occurred while updating the hotel: {ex.Message}");
			}
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: RoomService - DeleteAsync: {id}");

			try
			{
				var room = await _roomRepository.GetRoomByIdAsync(id);
				if (room == null)
				{
					_logger.Warning("Room not found");
					return new ApiResponse<int>(404, 0, "Room not found");
				}

				if (room.IsAvailable)
				{
					_logger.Warning("Room is currently booked and cannot be deleted");
					return new ApiResponse<int>(400, 0, "Room is currently booked and cannot be deleted");
				}

				var data = _mapper.Map<RoomResponseDTO>(room);
				room.DeletedAt = DateTime.UtcNow;
				var result = await _roomRepository.UpdateRoomAsync(room);
				await _publishEndpoint.Publish(new RoomEvent
				{
					Id = Guid.NewGuid(),
					Data = data,
					Type = "DELETE"
				});

				if (result <= 0)
				{
					_logger.Warning("Failed to delete room");
					return new ApiResponse<int>(400, -1, "Error occurred while deleting room");
				}

				// Invalidate cache
				await _cache.RemoveAsync($"Room_{id}");
				await _cache.RemoveAsync($"Room_{room.Name}");
				await _cache.RemoveAsync("Room_All");

				_logger.Information($"End: RoomService - DeleteAsync: {id} - Successfully deleted");
				return new ApiResponse<int>(200, id, "Room deleted successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in RoomService - DeleteAsync: {ex.Message}", ex);
				return new ApiResponse<int>(500, 0, $"An error occurred while deleting the room: {ex.Message}");
			}
		}

		public async Task<ApiResponse<RoomSearchResponseDTO>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest)
		{
			_logger.Information("Begin: RoomService - SearchRoomsAsync");
			try
			{
				var result = await _roomRepository.SearchRoomsAsync(searchRequest);
				var data = _mapper.Map<List<RoomResponseDTO>>(result.Tours);

				var response = new RoomSearchResponseDTO
				{
					Rooms = data,
					TotalItems = result.TotalItems,
					MinPrice = result.MinPrice,
					MaxPrice = result.MaxPrice,
					PageNumber = result.PageNumber,
					PageSize = result.PageSize
				};

				_logger.Information("End: RoomService - SearchRoomsAsync");
				return new ApiResponse<RoomSearchResponseDTO>(200, response, "Rooms retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in RoomService - SearchRoomsAsync: {ex.Message}", ex);
				return new ApiResponse<RoomSearchResponseDTO>(500, null, $"An error occurred while searching for rooms: {ex.Message}");
			}
		}
	}
}
