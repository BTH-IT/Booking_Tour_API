using Booking.API.Entities;
using Booking.API.Persistence;
using Contracts.Domains.Interfaces;

namespace Booking.API.Repositories.Interfaces
{
	public interface IBookingRoomRepository : IRepositoryBase<BookingRoom, int, BookingDbContext>
	{
		Task<IEnumerable<BookingRoom>> GetBookingRoomsAsync();
		Task<BookingRoom> GetBookingRoomByIdAsync(int id);
		Task CreateBookingRoomAsync(BookingRoom bookingRoom);
		Task UpdateBookingRoomAsync(BookingRoom bookingRoom);
		Task DeleteBookingRoomAsync(int id);
	}
}
