using Shared.DTOs;
using Shared.Helper;

namespace Room.API.Services.Interfaces
{
	public interface IHotelService
	{
		Task<ApiResponse<List<HotelResponseDTO>>> GetAllAsync();
		Task<ApiResponse<HotelResponseDTO>> GetByIdAsync(int id);
		Task<ApiResponse<HotelResponseDTO>> GetByNameAsync(string name);
		Task<ApiResponse<HotelResponseDTO>> CreateAsync(HotelRequestDTO item);
		Task<ApiResponse<HotelResponseDTO>> UpdateAsync(HotelRequestDTO item);
		Task<ApiResponse<int>> DeleteAsync(int id);
	}
}
