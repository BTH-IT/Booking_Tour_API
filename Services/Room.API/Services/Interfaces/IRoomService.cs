using Shared.DTOs;
using Shared.Helper;

namespace Room.API.Services.Interfaces
{
	public interface IRoomService
	{
		Task<ApiResponse<List<RoomResponseDTO>>> GetAllAsync();
		Task<ApiResponse<RoomResponseDTO>> GetByIdAsync(int id);
		Task<ApiResponse<RoomResponseDTO>> GetByNameAsync(string name);
		Task<ApiResponse<RoomResponseDTO>> CreateAsync(RoomRequestDTO item);
		Task<ApiResponse<RoomResponseDTO>> UpdateAsync(RoomRequestDTO item);
		Task<ApiResponse<int>> DeleteAsync(int id);
		Task<ApiResponse<PagedRoomResponseDTO>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest);
	}
}
