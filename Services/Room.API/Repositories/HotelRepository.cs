using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Persistence;
using Room.API.Repositories.Interfaces;

namespace Room.API.Repositories
{
	public class HotelRepository : RepositoryBase<Hotel, int, RoomDbContext>, IHotelRepository
	{
		public HotelRepository(RoomDbContext dbContext, IUnitOfWork<RoomDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}

<<<<<<< HEAD
		public async Task<IEnumerable<Hotel>> GetHotelsAsync() =>
			await FindByCondition(h => h.DeletedAt == null, false, h => h.Rooms.Where(tr => tr.DeletedAt == null))
				.OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public Task<Hotel?> GetHotelByNameAsync(string name) =>
			FindByCondition(h => h.Name.Equals(name) && h.DeletedAt == null, false, h => h.Rooms.Where(tr => tr.DeletedAt == null)).SingleOrDefaultAsync();

		public Task<Hotel?> GetHotelByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.Rooms.Where(tr => tr.DeletedAt == null)).SingleOrDefaultAsync();

		public Task<int> CreateHotelAsync(Hotel hotel) => CreateAsync(hotel);

		public Task<int> UpdateHotelAsync(Hotel hotel) => UpdateAsync(hotel);
=======
		public Task CreateHotelAsync(Hotel hotel) => CreateAsync(hotel);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

		public async Task DeleteHotelAsync(int id)
		{
			var hotel = await GetHotelByIdAsync(id);
			if (hotel != null)
			{
				hotel.DeletedAt = DateTime.UtcNow;

				await UpdateAsync(hotel);
			}
		}
<<<<<<< HEAD
=======

		public Task<Hotel> GetHotelByNameAsync(string name) =>
			FindByCondition(h => h.Name.Equals(name) && h.DeletedAt == null, false, h => h.Rooms).SingleOrDefaultAsync();

		public Task<Hotel> GetHotelByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.Rooms).SingleOrDefaultAsync();

		public async Task<IEnumerable<Hotel>> GetHotelsAsync() =>
			await FindByCondition(h => h.DeletedAt == null).ToListAsync();

		public Task UpdateHotelAsync(Hotel hotel) => UpdateAsync(hotel);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}
}
