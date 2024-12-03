using AutoMapper;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
=======
using Newtonsoft.Json;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;
<<<<<<< HEAD
using EventBus.IntergrationEvents.Events;
using MassTransit;

namespace Room.API.Services
=======

namespace Room.API.Services 
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
{
	public class RoomService : IRoomService
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IHotelRepository _hotelRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
<<<<<<< HEAD
		private readonly IPublishEndpoint _publishEndpoint;

		public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper, ILogger logger, IPublishEndpoint publishEndpoint)
=======

		public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper, ILogger logger)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			_roomRepository = roomRepository;
			_hotelRepository = hotelRepository;
			_mapper = mapper;
			_logger = logger;
<<<<<<< HEAD
			_publishEndpoint = publishEndpoint;
		}

		public async Task<ApiResponse<List<RoomResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: RoomService - GetAllAsync");

			try
			{
				var rooms = await _roomRepository.GetRoomsAsync();

				if (!rooms?.Any() ?? true)
				{
					_logger.Warning("No rooms");
					return new ApiResponse<List<RoomResponseDTO>>(204, null, "No rooms");
				}

				var data = _mapper.Map<List<RoomResponseDTO>>(rooms);

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
				var room = await _roomRepository.GetRoomByIdAsync(id);
				if (room == null || room.DeletedAt != null)
				{
					_logger.Warning($"Room with id {id} not found or deleted");
					return new ApiResponse<RoomResponseDTO>(404, null, $"Room with id {id} not found or deleted");
				}

				var data = _mapper.Map<RoomResponseDTO>(room);

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
				var room = await _roomRepository.GetRoomByNameAsync(name);
				if (room == null || room.DeletedAt != null)
				{
					_logger.Warning($"Room with name {name} not found or deleted");
					return new ApiResponse<RoomResponseDTO>(404, null, $"Room with name {name} not found or deleted");
				}

				var data = _mapper.Map<RoomResponseDTO>(room);

				_logger.Information("End: RoomService - GetByNameAsync");
				return new ApiResponse<RoomResponseDTO>(200, data, "Room data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in RoomService - GetByNameAsync: {ex.Message}", ex);
				return new ApiResponse<RoomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		}

		public async Task<ApiResponse<RoomResponseDTO>> CreateAsync(RoomRequestDTO item)
		{
			_logger.Information("Begin: RoomService - CreateAsync");

<<<<<<< HEAD
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

				_logger.Information("End: RoomService - UpdateAsync");
				return new ApiResponse<RoomResponseDTO>(200, data, "Room updated successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Unexpected error in RoomService - UpdateAsync: {ex.Message}", ex);
				return new ApiResponse<RoomResponseDTO>(500, null, $"An error occurred while updating the hotel: {ex.Message}");
			}
=======
			var existingRoom = await _roomRepository.GetRoomByNameAsync(item.Name);
			if (existingRoom != null)
			{
				return new ApiResponse<RoomResponseDTO>(400, null, "Room already exists");
			}

			var roomEntity = _mapper.Map<RoomEntity>(item);
			roomEntity.CreatedAt = DateTime.UtcNow;
			var newId = await _roomRepository.CreateAsync(roomEntity);

			var createdRoom = await _roomRepository.GetRoomByIdAsync(newId);
			var data = _mapper.Map<RoomResponseDTO>(createdRoom);

			_logger.Information("End: RoomService - CreateAsync");
			return new ApiResponse<RoomResponseDTO>(200, data, "Room created successfully");
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: RoomService - DeleteAsync: {id}");

<<<<<<< HEAD
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
=======
			var room = await _roomRepository.GetRoomByIdAsync(id);
			if (room == null)
			{
				return new ApiResponse<int>(404, 0, "Room not found");
			}

			if (room.IsAvailable)
			{
				return new ApiResponse<int>(400, 0, "Room is currently booked and cannot be deleted");
			}

			room.DeletedAt = DateTime.UtcNow;
			await _roomRepository.UpdateAsync(room);

			_logger.Information($"End: RoomService - DeleteAsync: {id} - Successfully deleted");
			return new ApiResponse<int>(200, id, "Room deleted successfully");
		}

		public async Task<ApiResponse<List<RoomResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: RoomService - GetAllAsync");

			var rooms = await _roomRepository.FindAll(false, room => room.Hotel)
											 .Where(r => r.DeletedAt == null)
											 .ToListAsync();

			var data = _mapper.Map<List<RoomResponseDTO>>(rooms);

			_logger.Information("End: RoomService - GetAllAsync");

			return new ApiResponse<List<RoomResponseDTO>>(200, data, "Data retrieved successfully");
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information("Begin: RoomService - GetByIdAsync");

			var room = await _roomRepository.GetRoomByIdAsync(id);
			if (room == null || room.DeletedAt != null)
			{
				return new ApiResponse<RoomResponseDTO>(404, null, "Room not found");
			}
			var data = _mapper.Map<RoomResponseDTO>(room);

			_logger.Information("End: RoomService - GetByIdAsync");
			return new ApiResponse<RoomResponseDTO>(200, data, "Room data retrieved successfully");
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information("Begin: RoomService - GetByNameAsync");

			var room = await _roomRepository.GetRoomByNameAsync(name);
			if (room == null || room.DeletedAt != null)
			{
				return new ApiResponse<RoomResponseDTO>(404, null, "Room not found");
			}

			var data = _mapper.Map<RoomResponseDTO>(room);

			_logger.Information("End: RoomService - GetByNameAsync");
			return new ApiResponse<RoomResponseDTO>(200, data, "Room data retrieved successfully");
		}

		public async Task<ApiResponse<RoomResponseDTO>> UpdateAsync(int id, RoomRequestDTO item)
		{
			try
			{
				_logger.Information("Begin: RoomService - UpdateAsync");

				var room = await _roomRepository.GetRoomByIdAsync(id);

				if (room == null || room.DeletedAt != null)
				{
					return new ApiResponse<RoomResponseDTO>(404, null, "Room not found");
				}

				if (await _roomRepository.FindByCondition(r => r.Name.Equals(item.Name) && r.Id != id && r.DeletedAt == null).FirstOrDefaultAsync() != null)
				{
					return new ApiResponse<RoomResponseDTO>(400, null, "Room name already exists");
				}

				var existingReviews = room.ReviewList ?? new List<ReviewRoom>();

				room = _mapper.Map<RoomEntity>(item);
				room.Id = id;
				room.ReviewList = existingReviews; 
				room.UpdatedAt = DateTime.UtcNow;

				var result = await _roomRepository.UpdateAsync(room);

				if (result > 0)
				{
					var updatedRoom = await _roomRepository.GetRoomByIdAsync(id);
					var data = _mapper.Map<RoomResponseDTO>(updatedRoom);

					_logger.Information("End: RoomService - UpdateAsync");
					return new ApiResponse<RoomResponseDTO>(200, data, "Room updated successfully");
				}

				_logger.Information("End: RoomService - UpdateAsync");
				return new ApiResponse<RoomResponseDTO>(400, null, "Error occurred while updating room");
			}
			catch (Exception ex)
			{
				_logger.Error($"Unexpected error occurred while updating room: {ex.Message}");
				return new ApiResponse<RoomResponseDTO>(500, null, "An unexpected error occurred while updating room.");
			}
		}

		public async Task<ApiResponse<PagedRoomResponseDTO>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest)
		{
			_logger.Information("Begin: RoomService - SearchRoomsAsync");

			var pagedResult = await _roomRepository.SearchRoomsAsync(searchRequest);

			var data = _mapper.Map<List<RoomResponseDTO>>(pagedResult.Items);

			var pagedResponse = new PagedRoomResponseDTO
			{
				Rooms = data,
				TotalItems = pagedResult.TotalItems,
				PageNumber = pagedResult.PageNumber,
				PageSize = pagedResult.PageSize
			};

			_logger.Information("End: RoomService - SearchRoomsAsync");

			return new ApiResponse<PagedRoomResponseDTO>(200, pagedResponse, "Rooms retrieved successfully");
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		}
	}
}
