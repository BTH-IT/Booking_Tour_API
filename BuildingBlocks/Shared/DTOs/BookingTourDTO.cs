namespace Shared.DTOs
{
    public class BookingTourResponseDTO
    {
        public int Id { get; set; }
		public int UserId { get; set; }
		public int ScheduleId { get; set; }
		public int Seats { get; set; }
        public bool IsLunch { get; set; }
        public bool IsTip { get; set; }
        public bool IsEntranceTicket { get; set; }
        public string Status { get; set; }
        public decimal PriceTotal { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public UserResponseDTO User { get; set; }
		public ScheduleResponseDTO Schedule { get; set; }
		public List<TravellerDTO> Travellers { get; set; }
    }

    public class BookingTourRequestDTO
    {
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public int Seats { get; set; }
        public bool IsLunch { get; set; }
        public bool IsTip { get; set; }
        public bool IsEntranceTicket { get; set; }
        public string Status { get; set; }
        public decimal PriceTotal { get; set; }
		public List<TravellerDTO> Travellers { get; set; }
    }
	public class TravellerDTO
	{
		public string Gender { get; set; }
		public string Fullname { get; set; }
		public sbyte Age { get; set; }
		public string Phone { get; set; }
	}
}
