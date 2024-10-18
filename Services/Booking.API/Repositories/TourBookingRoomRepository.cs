using Booking.API.Entities;
using Booking.API.Persistence;
using Booking.API.Repositories.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Repositories
{
	public class TourBookingRoomRepository : RepositoryBase<TourBookingRoom, int, BookingDbContext>, ITourBookingRoomRepository
	{
		public TourBookingRoomRepository(BookingDbContext dbContext, IUnitOfWork<BookingDbContext> unitOfWork) : base(dbContext, unitOfWork)
		{
		}

		public async Task<IEnumerable<TourBookingRoom>> GetTourBookingRoomsAsync() =>
			await FindByCondition(h => h.DeletedAt == null, false, h => h.BookingTour).ToListAsync();

		public Task<TourBookingRoom> GetTourBookingRoomByIdAsync(int id) =>
			FindByCondition(h => h.Id.Equals(id) && h.DeletedAt == null, false, h => h.BookingTourId).SingleOrDefaultAsync();

		public Task<TourBookingRoom> GetTourBookingRoomByBookingTourIdAsync(int id) =>
			FindByCondition(h => h.BookingTourId.Equals(id) && h.DeletedAt == null, false, h => h.BookingTourId).SingleOrDefaultAsync();

		public Task<int> CreateTourBookingRoomAsync(TourBookingRoom tourBookingRoom) => CreateAsync(tourBookingRoom);

		public Task<int> UpdateTourBookingRoomAsync(TourBookingRoom tourBookingRoom) => UpdateAsync(tourBookingRoom);

		public async Task DeleteTourBookingRoomAsync(int id)
		{
			var TourBookingRoom = await GetTourBookingRoomByIdAsync(id);
			if (TourBookingRoom != null)
			{
				TourBookingRoom.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(TourBookingRoom);
			}
		}
	}
}
