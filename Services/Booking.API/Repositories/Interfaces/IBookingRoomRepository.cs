using Booking.API.Entities;
using Booking.API.Persistence;
using Contracts.Domains.Interfaces;

namespace Booking.API.Repositories.Interfaces
{
    public interface IBookingRoomRepository : IRepositoryBase<BookingRoom, int, BookingDbContext>
    {
        void AddDetailBookingRooms(List<DetailBookingRoom> detailBookingRooms);
        void RemoveDetailBookingRooms(List<DetailBookingRoom> detailBookingRooms);
        Task<IEnumerable<BookingRoom>> GetBookingRoomsAsync();
        Task<BookingRoom> GetBookingRoomByIdAsync(int id);
        Task<BookingRoom> GetBookingRoomByNameAsync(string name);
        Task CreateBookingRoomAsync(BookingRoom bookingRoom);
        Task UpdateBookingRoomAsync(BookingRoom bookingRoom);
        Task DeleteBookingRoomAsync(int id);
    }
}
