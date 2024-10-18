using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Persistence;
using Room.API.Repositories.Interfaces;
using Shared.DTOs;
using Shared.Helper;

namespace Room.API.Repositories
{
	public class RoomRepository : RepositoryBase<RoomEntity, int, RoomDbContext>, IRoomRepository
	{
		public RoomRepository(RoomDbContext dbContext, IUnitOfWork<RoomDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}

		public async Task<IEnumerable<RoomEntity>> GetRoomsAsync() =>
			await FindByCondition(r => r.DeletedAt == null, false, r => r.Hotel).ToListAsync();

		public Task<RoomEntity> GetRoomByIdAsync(int id) =>
			 FindByCondition(r => r.Id == id && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

		public async Task<IEnumerable<RoomEntity>> GetRoomByHotelIdAsync(int id) =>
			await FindByCondition(r => r.HotelId == id && r.DeletedAt == null, false, r => r.Hotel).ToListAsync();

		public Task<RoomEntity> GetRoomByNameAsync(string name) =>
			 FindByCondition(r => r.Name == name && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

		public Task<int> CreateRoomAsync(RoomEntity room) => CreateAsync(room);

		public Task<int> UpdateRoomAsync(RoomEntity room) => UpdateAsync(room);

		public async Task DeleteRoomAsync(int id)
		{
			var room = await GetRoomByIdAsync(id);
			if (room != null)
			{
				room.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(room);
			}
		}

		public async Task<PagedResult<RoomEntity>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest)
		{
			var query = FindByCondition(r => r.DeletedAt == null, false, r => r.Hotel);

			if (!string.IsNullOrEmpty(searchRequest.Name))
			{
				query = query.Where(r => r.Name.Contains(searchRequest.Name));
			}

			if (!string.IsNullOrEmpty(searchRequest.Location))
			{
				query = query.Where(r => r.Hotel.Location.Contains(searchRequest.Location));
			}

			if (searchRequest.MaxGuests.HasValue)
			{
				query = query.Where(r => r.MaxGuests >= searchRequest.MaxGuests);
			}

			if (searchRequest.MinPrice.HasValue)
			{
				query = query.Where(r => r.Price >= searchRequest.MinPrice);
			}

			if (searchRequest.MaxPrice.HasValue)
			{
				query = query.Where(r => r.Price <= searchRequest.MaxPrice);
			}

			switch (searchRequest.SortBy?.ToLower())
			{
				case "price":
					query = searchRequest.SortOrder == "desc"
						? query.OrderByDescending(r => r.Price)
						: query.OrderBy(r => r.Price);
					break;
				case "maxguests":
					query = searchRequest.SortOrder == "desc"
						? query.OrderByDescending(r => r.MaxGuests)
						: query.OrderBy(r => r.MaxGuests);
					break;
				case "name":
					query = searchRequest.SortOrder == "desc"
						? query.OrderByDescending(r => r.Name)
						: query.OrderBy(r => r.Name);
					break;
				default:
					query = query.OrderBy(r => r.Name);
					break;
			}

			var totalItems = await query.CountAsync();
			var rooms = await query
				.Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
				.Take(searchRequest.PageSize)
				.ToListAsync();

			return new PagedResult<RoomEntity>(rooms, totalItems, searchRequest.PageNumber, searchRequest.PageSize);
		}
	}
}
