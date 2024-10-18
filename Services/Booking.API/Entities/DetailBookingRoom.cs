using Contracts.Domains;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
    [Table("DetailBookingRooms")]
    public class DetailBookingRoom : EntityBase<int>
    {
        public int BookingId { get; set; }  
        public int RoomId { get; set; } 
        public double Price { get; set; }   
        public int Adults { get; set; } 
        public int Children { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public BookingRoom? BookingRoom { get; set; }
    }
}
