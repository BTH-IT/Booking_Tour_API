using Booking.API.Entities;
using Booking.API.Persistence;
using Booking.API.Repositories.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;

namespace Booking.API.Repositories
{
    public class BookingTourRepository : RepositoryBase<BookingTour,int , BookingDbContext> ,IBookingTourRepository
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
}
