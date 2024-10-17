using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CreateBookingRoomOrderDto
    {
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double PriceTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int NumberOfPeople { get; set; }
        public List<BookingRoomOrderDetailDto> BookingRoomDetails   { get; set; }
    }
    public class BookingRoomOrderDetailDto
    {
        public int RoomId { get; set; }  
        public decimal PriceTotal { get; set; }
        public int Adults {  get; set; }
        public int Children { get; set; }
    }
}
