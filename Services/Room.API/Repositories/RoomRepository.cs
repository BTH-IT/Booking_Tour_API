using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Persistence;
using Room.API.Repositories.Interfaces;

namespace Room.API.Repositories
{
	public class RoomRepository : RepositoryBase<RoomEntity, int, RoomDbContext>, IRoomRepository
	{
		public RoomRepository(RoomDbContext dbContext, IUnitOfWork<RoomDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}
		public Task CreateRoomAsync(RoomEntity room) => CreateAsync(room);

		public async Task DeleteRoomAsync(int id)
		{
			var room = await GetRoomByIdAsync(id);
			if (room != null)
			{
				await DeleteAsync(room);
			}
		}

		public Task<RoomEntity> GetRoomByIdAsync(int id) =>
			FindByCondition(r => r.Id.Equals(id), false, r => r.Hotel).SingleOrDefaultAsync();

		public Task<RoomEntity> GetRoomByNameAsync(string name) =>
			FindByCondition(r => r.Name.Equals(name), false, r => r.Hotel).SingleOrDefaultAsync();

		public async Task<IEnumerable<RoomEntity>> GetRoomsAsync() => await FindAll().ToListAsync();

		public Task UpdateRoomAsync(RoomEntity room) => UpdateAsync(room);
	}
}
