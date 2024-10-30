namespace Shared.DTOs
{
    public class TourRoomRequestDTO
    {
        public int RoomId { get; set; }
    }

    public class TourRoomResponseDTO
    {
        public int Id { get; set; }
        public int TourID { get; set; }
        public int RoomId { get; set; }
        //public RoomResponseDTO Room { get; set; } // Nếu cần thông tin chi tiết về phòng
    }
}
