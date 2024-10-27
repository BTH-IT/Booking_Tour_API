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
        public bool Umbrella { get; set; }
        public bool IsCleaningFee { get; set; }
        public bool IsTip { get; set; }
        public bool IsEntranceTicket { get; set; }
        public sbyte PaymentMethod { get; set; }
        public decimal PriceTotal { get; set; }
        public bool Status { get; set; }
        public float Coupon { get; set; }
        public List<TourBookingRoomRequestDTO> TourBookingRooms { get; set; }
        public List<TravellerDTO> Travellers { get; set; }
    }
}
