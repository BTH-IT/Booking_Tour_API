using Contracts.Domains;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
    [Table("BookingRooms")]
    public class BookingRoom : EntityBase<int>
    {
        public string UserId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double PriceTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }    
        public int NumberOfPeople { get; set; }
        public ICollection<DetailBookingRoom>?  DetailBookingRooms { get; set; }    
    }
}
