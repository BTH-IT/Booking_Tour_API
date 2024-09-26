using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Room.API.Services
{
	public class RoomService : IRoomService
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public RoomService(IRoomRepository roomRepository, IMapper mapper, ILogger logger)
		{
			_roomRepository = roomRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<ApiResponse<int>> CreateAsync(RoomRequestDTO item)
		{
			_logger.Information("Begin: RoomService - CreateAsync");

			var existingRoom = await _roomRepository.GetRoomByNameAsync(item.Name);
			if (existingRoom != null)
			{
				return new ApiResponse<int>(400, -1, "Room already exists");
			}

			var roomEntity = _mapper.Map<RoomEntity>(item);
			var newId = await _roomRepository.CreateAsync(roomEntity);

			_logger.Information("End: RoomService - CreateAsync");
			return new ApiResponse<int>(200, newId, "Room created successfully");
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: RoomService - DeleteAsync: {id}");

			var room = await _roomRepository.GetRoomByIdAsync(id);
			if (room == null)
			{
				return new ApiResponse<int>(404, 0, "Room not found");
			}

			if (!room.IsAvailable)
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

			_logger.Information("Mapping list of rooms to DTO");
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


		public async Task<ApiResponse<RoomResponseDTO>> UpdateAsync(RoomRequestDTO item)
		{
			try
			{
				_logger.Information("Begin: RoomService - UpdateAsync");

				var room = await _roomRepository.GetRoomByIdAsync(item.Id);
				if (room == null || room.DeletedAt != null)
				{
					return new ApiResponse<RoomResponseDTO>(404, null, "Room not found");
				}

				if (await _roomRepository.FindByCondition(r => r.Name.Equals(item.Name) && r.Id != item.Id && r.DeletedAt == null).FirstOrDefaultAsync() != null)
				{
					return new ApiResponse<RoomResponseDTO>(400, null, "Room name already exists");
				}

				room = _mapper.Map<RoomEntity>(item);
				var result = await _roomRepository.UpdateAsync(room);

				if (result > 0)
				{
					_logger.Information("End: RoomService - UpdateAsync");
					return new ApiResponse<RoomResponseDTO>(200, _mapper.Map<RoomResponseDTO>(room), "Room updated successfully");
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
	}
}
