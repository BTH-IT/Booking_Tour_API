using Booking.API.Entities;
using Booking.API.Persistence;
using Booking.API.Repositories.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Helper;
using System.Xml.Linq;

namespace Booking.API.Repositories
{
    public class BookingRoomRepository :RepositoryBase<BookingRoom, int, BookingDbContext> ,IBookingRoomRepository
    {
        private readonly BookingDbContext _context;
        public BookingRoomRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
            _context = dbContext;
        }
        public void AddDetailBookingRooms(List<DetailBookingRoom> detailBookingRooms)
        {
            _context.AddRange(detailBookingRooms);
        }
        public Task CreateBookingRoomAsync(BookingRoom bookingRoom)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteBookingRoomAsync(int id)
        {
            var bookingRoom = await GetBookingRoomByIdAsync(id);
            if (bookingRoom != null)
            {
                bookingRoom.DeletedAt = DateTime.UtcNow;

                await UpdateAsync(bookingRoom);
            }

        }
        public Task<BookingRoom> GetBookingRoomByIdAsync(int id) => 
            FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.DetailBookingRooms).SingleOrDefaultAsync();
        public Task<BookingRoom> GetBookingRoomByNameAsync(string userId) =>
            FindByCondition(h => h.UserId.Equals(userId) && h.DeletedAt == null, false, h => h.DetailBookingRooms).SingleOrDefaultAsync();
        public async Task<IEnumerable<BookingRoom>> GetBookingRoomsAsync() => 
            await FindByCondition(h => h.DeletedAt == null).ToListAsync();
        public void RemoveDetailBookingRooms(List<DetailBookingRoom> detailBookingRooms)
        {
            _context.RemoveRange(detailBookingRooms);
        }
        public Task UpdateBookingRoomAsync(BookingRoom bookingRoom) => UpdateAsync(bookingRoom);
    }
}
