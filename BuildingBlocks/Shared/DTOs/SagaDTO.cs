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
        public List<BookingRoomOrderDetailDto> BookingRoomDetails   { get; set; }
    }
    public class BookingRoomOrderDetailDto
    {
        public int RoomId { get; set; }  
        public double Price { get; set; }
        public int Adults {  get; set; }
        public int Children { get; set; }
    }
}
