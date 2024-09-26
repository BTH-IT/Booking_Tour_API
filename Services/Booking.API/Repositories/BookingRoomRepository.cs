using Booking.API.Entities;
using Booking.API.Persistence;
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Repositories
{
    public class BookingRoomRepository :RepositoryBase<BookingRoom, int, BookingDbContext>
    {
        private readonly BookingDbContext _context;
    public BookingRoomRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
    {
        _context = dbContext;
    }
}
}
