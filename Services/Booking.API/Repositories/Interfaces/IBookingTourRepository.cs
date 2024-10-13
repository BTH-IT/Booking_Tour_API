using Booking.API.Entities;
using Booking.API.Persistence;
using Contracts.Domains.Interfaces;

namespace Booking.API.Repositories.Interfaces
{
    public interface IBookingTourRepository : IRepositoryBase<BookingTour,int,BookingDbContext>
    {
        void AddTourBookingRooms(List<TourBookingRoom> tourBookingRooms);
        void RemoveTourBookingRooms(List<TourBookingRoom> tourBookingRooms);
    }
}
