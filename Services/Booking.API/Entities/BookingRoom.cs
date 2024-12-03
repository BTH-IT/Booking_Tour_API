using Contracts.Domains;
<<<<<<< HEAD
using Contracts.Domains.Interfaces;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
    [Table("BookingRooms")]
<<<<<<< HEAD
    public class BookingRoom : EntityBase<int> , IDateTracking
    {
        public int UserId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
		public int NumberOfPeople { get; set; }
        public double PriceTotal { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }   
        public DateTime? DeletedAt { get; set; }
=======
    public class BookingRoom : EntityBase<int>
    {
        public string UserId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double PriceTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }    
        public int NumberOfPeople { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public ICollection<DetailBookingRoom>?  DetailBookingRooms { get; set; }    
    }
}
