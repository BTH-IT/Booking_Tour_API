using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Persistence;
using Room.API.Repositories.Interfaces;
using Shared.DTOs;

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
				room.DeletedAt = DateTime.UtcNow; 
				await UpdateAsync(room);
			}
		}

		public Task<RoomEntity> GetRoomByIdAsync(int id) =>
			 FindByCondition(r => r.Id == id && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

		public Task<RoomEntity> GetRoomByNameAsync(string name) =>
			 FindByCondition(r => r.Name == name && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

		public async Task<IEnumerable<RoomEntity>> GetRoomsAsync() =>
			await FindAll().Where(r => r.DeletedAt == null).ToListAsync();

		public async Task<IEnumerable<RoomEntity>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest)
		{
			var query = FindAll().Where(r => r.DeletedAt == null);

			if (searchRequest.Guests != null)
			{
				var totalGuests = searchRequest.Guests.Adults + searchRequest.Guests.Children;
				query = query.Where(r => r.MaxGuests >= totalGuests);
			}

			if (searchRequest.Facilities?.Any() == true)
			{
				var allRooms = await query.ToListAsync(); 
				var filteredRooms = allRooms.Where(r => searchRequest.Facilities.All(f => r.RoomAmenitiesList.Any(ra => ra.Title == f))).ToList();
				return filteredRooms;
			}

			if (!string.IsNullOrEmpty(searchRequest.RoomSize))
			{
				query = FilterByRoomSize(query, searchRequest.RoomSize);
			}

			return await query.ToListAsync();
		}

		private IQueryable<RoomEntity> FilterByRoomSize(IQueryable<RoomEntity> query, string roomSize)
		{
			return roomSize switch
			{
				"30-40" => query.Where(r => r.Size >= 30 && r.Size <= 40),
				"40-55" => query.Where(r => r.Size > 40 && r.Size <= 55),
				"55-80" => query.Where(r => r.Size > 55 && r.Size <= 80),
				"80+" => query.Where(r => r.Size > 80),
				_ => query
			};
		}

		public Task UpdateRoomAsync(RoomEntity room) => UpdateAsync(room);
	}
}
