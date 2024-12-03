using Contracts.Domains.Interfaces;
using Room.API.Entities;
using Room.API.Persistence;
using Shared.DTOs;
<<<<<<< HEAD
=======
using Shared.Helper;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

namespace Room.API.Repositories.Interfaces
{
	public interface IRoomRepository : IRepositoryBase<RoomEntity, int, RoomDbContext>
	{
		Task<IEnumerable<RoomEntity>> GetRoomsAsync();
<<<<<<< HEAD
		Task<RoomEntity?> GetRoomByIdAsync(int id);
		Task<RoomEntity?> GetRoomByNameAsync(string name);
		Task<IEnumerable<RoomEntity>> GetRoomByHotelIdAsync(int id);
		Task<int> CreateRoomAsync(RoomEntity room);
		Task<int> UpdateRoomAsync(RoomEntity room);
		Task DeleteRoomAsync(int id);
        Task<RoomSearchResult> SearchRoomsAsync(RoomSearchRequestDTO searchRequest);
=======
		Task<RoomEntity> GetRoomByIdAsync(int id);
		Task<RoomEntity> GetRoomByNameAsync(string name);
		Task CreateRoomAsync(RoomEntity room);
		Task UpdateRoomAsync(RoomEntity room);
		Task DeleteRoomAsync(int id);
		Task<PagedResult<RoomEntity>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}
}
