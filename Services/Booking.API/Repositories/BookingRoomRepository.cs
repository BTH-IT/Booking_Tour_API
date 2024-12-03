using Booking.API.Entities;
using Booking.API.Persistence;
<<<<<<< HEAD
using Booking.API.Repositories.Interfaces;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

<<<<<<< HEAD

namespace Booking.API.Repositories
{
    public class BookingRoomRepository :RepositoryBase<BookingRoom, int, BookingDbContext>, IBookingRoomRepository
	{
		public BookingRoomRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}

		public async Task<IEnumerable<BookingRoom>> GetBookingRoomsAsync() =>
			await FindByCondition(h => h.DeletedAt == null, false, h => h.DetailBookingRooms.Where(tr => tr.DeletedAt == null))
				.OrderByDescending(r => r.CreatedAt)
				.ToListAsync();

		public Task<BookingRoom> GetBookingRoomByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.DetailBookingRooms.Where(tr => tr.DeletedAt == null)).SingleOrDefaultAsync();

		public Task<int> CreateBookingRoomAsync(BookingRoom bookingRoom) => CreateAsync(bookingRoom);

		public Task<int> UpdateBookingRoomAsync(BookingRoom bookingRoom) => UpdateAsync(bookingRoom);

		public async Task DeleteBookingRoomAsync(int id)
		{
			var bookingRoom = await GetBookingRoomByIdAsync(id);
			if (bookingRoom != null)
			{
				if (bookingRoom.CheckOut.HasValue && DateTime.UtcNow > bookingRoom.CheckOut.Value)
				{
					bookingRoom.DeletedAt = DateTime.UtcNow;

					await UpdateAsync(bookingRoom);
				}
			}
		}
	}
=======
namespace Booking.API.Repositories
{
    public class BookingRoomRepository :RepositoryBase<BookingRoom, int, BookingDbContext>
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
        public void RemoveDetailBookingRooms(List<DetailBookingRoom> detailBookingRooms)
        {
            _context.RemoveRange(detailBookingRooms);
        }
    }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}
