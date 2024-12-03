using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Newtonsoft.Json;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Room.API.Entities;
using Room.API.Persistence;
using Room.API.Repositories.Interfaces;
using Shared.DTOs;
<<<<<<< HEAD
using System.Linq;
=======
using Shared.Helper;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

namespace Room.API.Repositories
{
	public class RoomRepository : RepositoryBase<RoomEntity, int, RoomDbContext>, IRoomRepository
	{
		public RoomRepository(RoomDbContext dbContext, IUnitOfWork<RoomDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}
<<<<<<< HEAD

        public async Task<IEnumerable<RoomEntity>> GetRoomsAsync() =>
			await FindByCondition(r => r.DeletedAt == null, false, r => r.Hotel)
				.OrderByDescending(r => r.CreatedAt)
				.ToListAsync();

        public Task<RoomEntity?> GetRoomByIdAsync(int id) =>
			 FindByCondition(r => r.Id == id && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

        public async Task<IEnumerable<RoomEntity>> GetRoomByHotelIdAsync(int id) =>
            await FindByCondition(r => r.HotelId == id && r.DeletedAt == null, false, r => r.Hotel)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public Task<RoomEntity?> GetRoomByNameAsync(string name) =>
			 FindByCondition(r => r.Name == name && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

		public Task<int> CreateRoomAsync(RoomEntity room) => CreateAsync(room);

		public Task<int> UpdateRoomAsync(RoomEntity room) => UpdateAsync(room);
=======
		public Task CreateRoomAsync(RoomEntity room) => CreateAsync(room);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

		public async Task DeleteRoomAsync(int id)
		{
			var room = await GetRoomByIdAsync(id);
			if (room != null)
			{
<<<<<<< HEAD
				room.DeletedAt = DateTime.UtcNow;
=======
				room.DeletedAt = DateTime.UtcNow; 
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
				await UpdateAsync(room);
			}
		}

<<<<<<< HEAD
        public async Task<RoomSearchResult> SearchRoomsAsync(RoomSearchRequestDTO searchRequest)
        {
            var query = FindByCondition(r => r.DeletedAt == null, false, r => r.Hotel);

            var minPrice = (await query.MinAsync(e => (decimal?)e.Price)) ?? 0m;
            var maxPrice = (await query.MaxAsync(e => (decimal?)e.Price)) ?? 0m;

            if (!string.IsNullOrEmpty(searchRequest.Name))
            {
                query = query.Where(r => r.Name.Contains(searchRequest.Name));
            }

            if (searchRequest.LocationCode != null && searchRequest.LocationCode.Any())
            {
                query = query.Where(r => searchRequest.LocationCode.Contains(r.Hotel.LocationCode.ToString()));
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

            if (searchRequest.HotelRules != null && searchRequest.HotelRules.Any())
            {
                query = query.Where(r => r.Hotel.HotelRules != null &&
                   EF.Functions.JsonContains(r.Hotel.HotelRules, JsonConvert.SerializeObject(searchRequest.HotelRules)));
            }

            if (searchRequest.HotelAmenities != null && searchRequest.HotelAmenities.Any())
            {
                query = query.Where(r => r.Hotel.HotelAmenities != null &&
                    EF.Functions.JsonContains(r.Hotel.HotelAmenities, JsonConvert.SerializeObject(searchRequest.HotelAmenities)));
            }

            if (searchRequest.RoomAmenities != null && searchRequest.RoomAmenities.Any())
            {
                query = query.Where(r => r.RoomAmenities != null &&
                    EF.Functions.JsonContains(r.RoomAmenities, JsonConvert.SerializeObject(searchRequest.RoomAmenities)));
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

            return new RoomSearchResult(rooms, totalItems, minPrice, maxPrice, searchRequest.PageNumber, searchRequest.PageSize);
        }
    }
=======
		public Task<RoomEntity> GetRoomByIdAsync(int id) =>
			 FindByCondition(r => r.Id == id && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

		public Task<RoomEntity> GetRoomByNameAsync(string name) =>
			 FindByCondition(r => r.Name == name && r.DeletedAt == null, false, r => r.Hotel).SingleOrDefaultAsync();

		public async Task<IEnumerable<RoomEntity>> GetRoomsAsync() =>
		    await FindByCondition(r => r.DeletedAt == null, false, r => r.Hotel).ToListAsync();

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

		public Task UpdateRoomAsync(RoomEntity room) => UpdateAsync(room);
	}
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}
