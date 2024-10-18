using Contracts.Domains.Interfaces;
using Room.API.Entities;
using Room.API.Persistence;
using Shared.DTOs;
using Shared.Helper;

namespace Room.API.Repositories.Interfaces
{
	public interface IRoomRepository : IRepositoryBase<RoomEntity, int, RoomDbContext>
	{
		Task<IEnumerable<RoomEntity>> GetRoomsAsync();
		Task<RoomEntity> GetRoomByIdAsync(int id);
		Task<RoomEntity> GetRoomByNameAsync(string name);
		Task<IEnumerable<RoomEntity>> GetRoomByHotelIdAsync(int id);
		Task<int> CreateRoomAsync(RoomEntity room);
		Task<int> UpdateRoomAsync(RoomEntity room);
		Task DeleteRoomAsync(int id);
		Task<PagedResult<RoomEntity>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest);
	}
}
