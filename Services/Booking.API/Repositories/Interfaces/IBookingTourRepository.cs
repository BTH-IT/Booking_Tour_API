using Booking.API.Entities;
using Booking.API.Persistence;
using Contracts.Domains.Interfaces;

namespace Booking.API.Repositories.Interfaces
{
	public interface IBookingTourRepository : IRepositoryBase<BookingTour, int, BookingDbContext>
	{
		Task<IEnumerable<BookingTour>> GetBookingToursAsync();
		Task<BookingTour> GetBookingTourByIdAsync(int id);
		Task CreateBookingTourAsync(BookingTour bookingTour);
		Task UpdateBookingTourAsync(BookingTour bookingTour);
		Task DeleteBookingTourAsync(int id);
	}
}
