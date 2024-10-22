using Booking.API.Entities;
using Booking.API.Persistence;
using Booking.API.Repositories.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Repositories
{
	public class BookingTourRepository : RepositoryBase<BookingTour, int, BookingDbContext>, IBookingTourRepository
	{
		public BookingTourRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}

		public async Task<IEnumerable<BookingTour>> GetBookingToursAsync() =>
			await FindByCondition(h => h.DeletedAt == null, false, h => h.TourBookingRooms).OrderByDescending(r => r.CreatedAt).ToListAsync();

		public Task<BookingTour> GetBookingTourByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.TourBookingRooms).SingleOrDefaultAsync();

		public Task<int> CreateBookingTourAsync(BookingTour bookingTour) => CreateAsync(bookingTour);

		public Task<int> UpdateBookingTourAsync(BookingTour bookingTour) => UpdateAsync(bookingTour);

		public async Task DeleteBookingTourAsync(int id)
		{
			var BookingTour = await GetBookingTourByIdAsync(id);
			if (BookingTour != null)
			{
				BookingTour.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(BookingTour);
			}
		}
	}
}
