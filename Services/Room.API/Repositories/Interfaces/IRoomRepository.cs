﻿using Contracts.Domains.Interfaces;
using Room.API.Entities;
using Room.API.Persistence;

namespace Room.API.Repositories.Interfaces
{
	public interface IRoomRepository : IRepositoryBase<RoomEntity, int, RoomDbContext>
	{
		Task<IEnumerable<RoomEntity>> GetRoomsAsync();
		Task<RoomEntity> GetRoomByIdAsync(int id);
		Task<RoomEntity> GetRoomByNameAsync(string name);
		Task CreateRoomAsync(RoomEntity room);
		Task UpdateRoomAsync(RoomEntity room);
		Task DeleteRoomAsync(int id);
	}
}