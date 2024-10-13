using Booking.API.Repositories.Interfaces;

namespace Booking.API.Services
{
    public class BookingTourService
    {
        private readonly IBookingTourRepository _repository;
        public BookingTourService(IBookingTourRepository repository)
        {
            _repository = repository; 
        }
        
    }
}
