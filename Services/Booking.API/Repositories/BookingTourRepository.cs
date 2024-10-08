using Booking.API.Entities;
using Booking.API.Persistence;
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
    }
}
