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
		Task<ApiResponse<RoomResponseDTO>> UpdateAsync(int id, RoomRequestDTO item);
		Task<ApiResponse<int>> DeleteAsync(int id);
<<<<<<< HEAD
		Task<ApiResponse<RoomSearchResponseDTO>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest);
=======
		Task<ApiResponse<PagedRoomResponseDTO>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}
}
