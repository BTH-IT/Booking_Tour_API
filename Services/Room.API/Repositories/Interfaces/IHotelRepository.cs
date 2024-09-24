using Contracts.Domains.Interfaces;
using Room.API.Entities;
using Room.API.Persistence;

namespace Room.API.Repositories.Interfaces
{
	public interface IHotelRepository: IRepositoryBase<Hotel,int, RoomDbContext>
	{
		Task<IEnumerable<Hotel>> GetHotelsAsync();
		Task<Hotel> GetHotelByIdAsync(int id);
		Task<Hotel> GetHotelByNameAsync(string name);
		Task CreateHotelAsync(Hotel hotel);
		Task UpdateHotelAsync(Hotel hotel);
		Task DeleteHotelAsync(int id);
	}
}
