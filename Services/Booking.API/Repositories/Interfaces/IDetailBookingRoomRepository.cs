using Booking.API.Entities;
using Booking.API.Persistence;
using Contracts.Domains.Interfaces;

namespace Booking.API.Repositories.Interfaces
{
	public interface IDetailBookingRoomRepository : IRepositoryBase<DetailBookingRoom, int, BookingDbContext>
	{
		Task<IEnumerable<DetailBookingRoom>> GetDetailBookingRoomsAsync();
		Task<DetailBookingRoom> GetDetailBookingRoomByIdAsync(int id);
		Task<DetailBookingRoom> GetDetailBookingRoomByBookingRoomIdAsync(int id);
		Task<int> CreateDetailBookingRoomAsync(DetailBookingRoom detailBookingRoom);
		Task<int> UpdateDetailBookingRoomAsync(DetailBookingRoom detailBookingRoom);
		Task DeleteDetailBookingRoomAsync(int id);
	}
}
