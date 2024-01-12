using BookingApi.Models;

namespace BookingApi.DTO
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public float Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Tour Tour { get; set; }
        public User User { get; set; }
    }

    public class ReviewRequestDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public float Rating { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int TourId { get; set; }
        public int UserId { get; set; }
    }
}
