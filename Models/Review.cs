using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApi.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Id is auto-increment
        public int Id { get; set; }

        public string Content { get; set; }

        [Range(0.0, 5.0)]
        public float Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Tour Tour { get; set; }
        public User User { get; set; }
    }
}
