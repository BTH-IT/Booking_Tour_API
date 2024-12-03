using Contracts.Domains.Interfaces;
using Room.API.Entities;
using Room.API.Persistence;

namespace Room.API.Repositories.Interfaces
{
	public interface IHotelRepository: IRepositoryBase<Hotel,int, RoomDbContext>
	{
		Task<IEnumerable<Hotel>> GetHotelsAsync();
<<<<<<< HEAD
		Task<Hotel?> GetHotelByIdAsync(int id);
		Task<Hotel?> GetHotelByNameAsync(string name);
		Task<int> CreateHotelAsync(Hotel hotel);
		Task<int> UpdateHotelAsync(Hotel hotel);
=======
		Task<Hotel> GetHotelByIdAsync(int id);
		Task<Hotel> GetHotelByNameAsync(string name);
		Task CreateHotelAsync(Hotel hotel);
		Task UpdateHotelAsync(Hotel hotel);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		Task DeleteHotelAsync(int id);
	}
}
