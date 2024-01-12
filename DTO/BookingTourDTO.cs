using BookingApi.Models;

namespace BookingApi.DTO
{
    public class BookingTourResponseDTO
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Schedule Schedule { get; set; }
        public int Seats { get; set; }
        public int Umbrella { get; set; }
        public bool IsCleaningFee { get; set; }
        public bool IsTip { get; set; }
        public bool IsEntranceTicket { get; set; }
        public bool Status { get; set; }
        public decimal PriceTotal { get; set; }
        public float Coupon { get; set; }
        public sbyte PaymentMethod { get; set; }
        public Traveller[] Travellers { get; set; }
    }

    public class BookingTourRequestDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public int Seats { get; set; }
        public int Umbrella { get; set; }
        public bool IsCleaningFee { get; set; }
        public bool IsTip { get; set; }
        public bool IsEntranceTicket { get; set; }
        public bool Status { get; set; }
        public decimal PriceTotal { get; set; }
        public float Coupon { get; set; }
        public sbyte PaymentMethod { get; set; }
        public Traveller[] Travellers { get; set; }
    }
}
