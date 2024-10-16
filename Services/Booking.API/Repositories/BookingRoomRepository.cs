using Booking.API.Entities;
using Booking.API.Persistence;
using Booking.API.Repositories.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Repositories
{
    public class BookingRoomRepository :RepositoryBase<BookingRoom, int, BookingDbContext>, IBookingRoomRepository
	{
		public BookingRoomRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}

		public async Task<IEnumerable<BookingRoom>> GetBookingRoomsAsync() =>
			await FindByCondition(h => h.DeletedAt == null, false, h => h.DetailBookingRooms).ToListAsync();

		public Task<BookingRoom> GetBookingRoomByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.DetailBookingRooms).SingleOrDefaultAsync();

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
}
