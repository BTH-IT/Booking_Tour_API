using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace BookingApi.Models
{
    public class Traveller
    {
        public bool Gender {get; set;}
        public string Fullname {get; set;}
        public sbyte Age { get; set;}
        public string Phone {get; set;}
    }

    [Table("BookingTour")]
    public class BookingTour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Id is auto-increment
        public int Id {get; set;}
        public User User {get; set;}
        public Schedule Schedule { get; set;}
        public int Seats {get; set;}
        public int Umbrella {get; set;}
        public bool IsCleaningFee {get; set;}
        public bool IsTip { get; set;}
        public bool IsEntranceTicket { get; set;}
        public bool Status {get; set;}
        public decimal PriceTotal {get; set;}
        public float Coupon {get; set;}
        public sbyte PaymentMethod {get; set;}
        [NotMapped]
        public Traveller[] TravellerList { get; set; }

        [Column(TypeName = "JSON")] // Use the appropriate database column type for JSON in your database
        public string Travellers
        {
            get => JsonConvert.SerializeObject(TravellerList);
            set => TravellerList = JsonConvert.DeserializeObject<Traveller[]>(value) ?? new Traveller[0];
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
