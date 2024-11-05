
namespace Shared.DTOs
{
    public class ScheduleRequestDTO
    {
        public int TourId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int AvailableSeats { get; set; }

	}

	public class ScheduleResponseDTO
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int AvailableSeats { get; set; }
		public TourResponseDTO Tour { get; set; }
	}

    public class ScheduleRoomRequestDTO
    {
        public int RoomId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

    }

    public class ScheduleRoomResponseDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public RoomResponseDTO Room { get; set; }
    }
}
