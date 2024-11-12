using Contracts.Domains;
using Contracts.Domains.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
    [Table("BookingRooms")]
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
        public ICollection<DetailBookingRoom>?  DetailBookingRooms { get; set; }    
    }
}
