using Contracts.Domains;
using Contracts.Domains.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
    [Table("TourBookingRooms")]
    public class TourBookingRoom : EntityBase<int>, IDateTracking
    {
        public int BookingTourId { get; set; }  
        public int RoomId { get; set; }
        public double Price { get; set; }   
        public int Adults { get; set; }
        public int Children { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
        public BookingTour? BookingTour { get; set; }
    }
}
