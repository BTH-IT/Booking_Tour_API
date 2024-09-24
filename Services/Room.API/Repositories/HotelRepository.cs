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

		public Task CreateHotelAsync(Hotel hotel) => CreateAsync(hotel);

		public async Task DeleteHotelAsync(int id)
		{
			var hotel = await GetHotelByIdAsync(id);
			if (hotel != null)
			{
				await DeleteAsync(hotel);
			}
		}
		public Task<Hotel> GetHotelByNameAsync(string name) =>
			FindByCondition(h => h.Name.Equals(name), false, h => h.Rooms).SingleOrDefaultAsync();

		public Task<Hotel> GetHotelByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id), false, h => h.Rooms).SingleOrDefaultAsync();

		public async Task<IEnumerable<Hotel>> GetHotelsAsync() => await FindAll().ToListAsync();

		public Task UpdateHotelAsync(Hotel hotel) => UpdateAsync(hotel);
	}
}
