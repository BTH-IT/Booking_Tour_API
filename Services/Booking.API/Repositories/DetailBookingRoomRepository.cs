using Booking.API.Entities;
using Booking.API.Persistence;
using Booking.API.Repositories.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Repositories
{
	public class DetailBookingRoomRepository : RepositoryBase<DetailBookingRoom, int, BookingDbContext>, IDetailBookingRoomRepository
	{
		public DetailBookingRoomRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}

		public async Task<IEnumerable<DetailBookingRoom>> GetDetailBookingRoomsAsync() =>
			await FindByCondition(h => h.DeletedAt == null, false, h => h.BookingRoom).ToListAsync();

		public Task<DetailBookingRoom> GetDetailBookingRoomByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.BookingRoom).SingleOrDefaultAsync();

		public Task<DetailBookingRoom> GetDetailBookingRoomByBookingRoomIdAsync(int id) =>
			FindByCondition(h => h.BookingId.Equals(id) && h.DeletedAt == null, false, h => h.BookingRoom).SingleOrDefaultAsync();

		public Task<int> CreateDetailBookingRoomAsync(DetailBookingRoom detailBookingRoom) => CreateAsync(detailBookingRoom);

		public Task<int> UpdateDetailBookingRoomAsync(DetailBookingRoom detailBookingRoom) => UpdateAsync(detailBookingRoom);

		public async Task DeleteDetailBookingRoomAsync(int id)
		{
			var detailBookingRoom = await GetDetailBookingRoomByIdAsync(id);
			if (detailBookingRoom != null)
			{
				detailBookingRoom.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(detailBookingRoom);
			}
		}
	}
}
