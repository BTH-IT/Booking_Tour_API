
namespace Shared.DTOs
{
    public class ScheduleRequestDTO
    {
        public int? Id { get; set; }
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
    }
}
