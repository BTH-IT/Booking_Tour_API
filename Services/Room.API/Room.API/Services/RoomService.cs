using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces ;
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

		public RoomService(IRoomRepository roomRepository,
			IMapper mapper,
			ILogger logger)
		{
			_roomRepository = roomRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<ApiResponse<int>> CreateAsync(RoomRequestDTO item)
		{
			_logger.Information("Begin : RoomService - CreateAsync");

			// Check if a room with the same name already exists
			var existingRoom = await _roomRepository.GetRoomByNameAsync(item.Name);
			if (existingRoom != null)
			{
				return new ApiResponse<int>(400, -1, "Phòng đã tồn tại");
			}

			var roomEntity = _mapper.Map<RoomEntity>(item);
			var newId = await _roomRepository.CreateAsync(roomEntity);

			_logger.Information("End : RoomService - CreateAsync");
			return new ApiResponse<int>(200, newId, "Tạo phòng thành công");
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin : RoomService - DeleteAsync : {id}");

			var room = await _roomRepository.GetRoomByIdAsync(id);
			if (room == null)
			{
				return new ApiResponse<int>(404, 0, "Không tìm thấy phòng cần xóa");
			}

			_roomRepository.Delete(room);
			var result = await _roomRepository.SaveChangesAsync();

			if (result > 0)
			{
				_logger.Information($"End : RoomService - DeleteAsync : {id} - Xóa thành công");
				return new ApiResponse<int>(200, result, "Xóa phòng thành công");
			}
			else
			{
				_logger.Information($"End : RoomService - DeleteAsync : {id} - Xóa thất bại");
				return new ApiResponse<int>(400, result, "Xóa phòng thất bại");
			}
		}

		public async Task<ApiResponse<List<RoomResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin : RoomService - GetAllAsync");

			var rooms = await _roomRepository.FindAll(false, room => room.Hotel).ToListAsync();
			_logger.Information("Mapping list of rooms to DTO");

			var data = _mapper.Map<List<RoomResponseDTO>>(rooms);

			_logger.Information("End : RoomService - GetAllAsync");
			return new ApiResponse<List<RoomResponseDTO>>(200, data, "Lấy dữ liệu thành công");
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin : RoomService - GetByIdAsync");

			var room = await _roomRepository.GetRoomByIdAsync(id);
			if (room == null)
			{
				return new ApiResponse<RoomResponseDTO>(404, null, "Không tìm thấy phòng");
			}

			var data = _mapper.Map<RoomResponseDTO>(room);

			_logger.Information("End : RoomService - GetByIdAsync");
			return new ApiResponse<RoomResponseDTO>(200, data, "Lấy dữ liệu phòng thành công");
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information($"Begin : RoomService - GetByNameAsync");

			var room = await _roomRepository.GetRoomByNameAsync(name);
			if (room == null)
			{
				return new ApiResponse<RoomResponseDTO>(404, null, "Không tìm thấy phòng");
			}

			var data = _mapper.Map<RoomResponseDTO>(room);

			_logger.Information("End : RoomService - GetByNameAsync");
			return new ApiResponse<RoomResponseDTO>(200, data, "Lấy dữ liệu phòng thành công");
		}

		public async Task<ApiResponse<RoomResponseDTO>> UpdateAsync(RoomRequestDTO item)
		{
			_logger.Information("Begin : RoomService - UpdateAsync");

			var room = await _roomRepository.GetRoomByIdAsync(item.Id);
			if (room == null)
			{
				return new ApiResponse<RoomResponseDTO>(404, null, "Không tìm thấy phòng");
			}

			// Check if a room with the same name already exists (excluding current room)
			if (await _roomRepository.FindByCondition(r => r.Name.Equals(item.Name) && r.Id != item.Id).FirstOrDefaultAsync() != null)
			{
				return new ApiResponse<RoomResponseDTO>(400, null, "Tên phòng đã tồn tại");
			}

			room = _mapper.Map<RoomEntity>(item);
			var result = await _roomRepository.UpdateAsync(room);

			if (result > 0)
			{
				_logger.Information("End : RoomService - UpdateAsync");
				return new ApiResponse<RoomResponseDTO>(200, _mapper.Map<RoomResponseDTO>(room), "Cập nhật thành công");
			}

			_logger.Information("End : RoomService - UpdateAsync");
			return new ApiResponse<RoomResponseDTO>(400, null, "Có lỗi xảy ra khi cập nhật");
		}
	}
}
