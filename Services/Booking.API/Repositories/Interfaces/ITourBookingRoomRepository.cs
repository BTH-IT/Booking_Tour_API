using Booking.API.Entities;
using Booking.API.Persistence;
using Contracts.Domains.Interfaces;

namespace Booking.API.Repositories.Interfaces
{
	public interface ITourBookingRoomRepository: IRepositoryBase<TourBookingRoom, int, BookingDbContext>
	{
		Task<IEnumerable<TourBookingRoom>> GetTourBookingRoomsAsync();
		Task<TourBookingRoom> GetTourBookingRoomByIdAsync(int id);
		Task<TourBookingRoom> GetTourBookingRoomByBookingTourIdAsync(int id);
		Task<int> CreateTourBookingRoomAsync(TourBookingRoom tourBookingRoom);
		Task<int> UpdateTourBookingRoomAsync(TourBookingRoom tourBookingRoom);
		Task DeleteTourBookingRoomAsync(int id);
	}
}
