namespace Shared.DTOs
{
    public class CreateBookingRoomOrderDto
    {
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public List<BookingRoomOrderDetailDto> BookingRoomDetails { get; set; }
    }
    public class BookingRoomOrderDetailDto
    {
        public int RoomId { get; set; }
        public double Price { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
    }
    public class CreateBookingTourOrderDto
    {
        public int ScheduleId { get; set; }
        public int Seats { get; set; }
        public bool IsLunch { get; set; }
        public bool IsTip { get; set; }
        public bool IsEntranceTicket { get; set; }
        public decimal PriceTotal { get; set; }
        public string Status { get; set; }
        public List<TravellerDTO> Travellers { get; set; }
    }
}
