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

		public async Task<IEnumerable<Hotel>> GetHotelsAsync() =>
			await FindByCondition(h => h.DeletedAt == null, false, h => h.Rooms).ToListAsync();

		public Task<Hotel> GetHotelByNameAsync(string name) =>
			FindByCondition(h => h.Name.Equals(name) && h.DeletedAt == null, false, h => h.Rooms).SingleOrDefaultAsync();

		public Task<Hotel> GetHotelByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.Rooms).SingleOrDefaultAsync();

		public Task CreateHotelAsync(Hotel hotel) => CreateAsync(hotel);

		public Task UpdateHotelAsync(Hotel hotel) => UpdateAsync(hotel);

		public async Task DeleteHotelAsync(int id)
			{
			var hotel = await GetHotelByIdAsync(id);
			if (hotel != null)
				{
				hotel.DeletedAt = DateTime.UtcNow;

				await UpdateAsync(hotel);
			}
		}
	}
}
