using Booking.API.Entities;
using Booking.API.Persistence;
<<<<<<< HEAD
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
			await FindByCondition(h => h.DeletedAt == null, false).OrderByDescending(r => r.CreatedAt).ToListAsync();

		public Task<BookingTour> GetBookingTourByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false).SingleOrDefaultAsync();

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
=======
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;

namespace Booking.API.Repositories
{
    public class BookingTourRepository : RepositoryBase<BookingTour,int , BookingDbContext>
    {
        private readonly BookingDbContext _context;
        public BookingTourRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
            _context = dbContext;
        }
        public void AddTourBookingRooms(List<TourBookingRoom> tourBookingRooms)
        {
            _context.TourBookingRooms.AddRange(tourBookingRooms);
        }
        public void RemoveTourBookingRooms(List<TourBookingRoom> tourBookingRooms)
        {
            _context.RemoveRange(tourBookingRooms);
        }
    }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}
