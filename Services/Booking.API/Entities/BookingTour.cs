using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
<<<<<<< HEAD
    
=======
    public class Traveller
    {
        public bool Gender { get; set; }
        public string Fullname { get; set; }
        public sbyte Age { get; set; }
        public string Phone { get; set; }
    }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    [Table("BookingTours")]
    public class BookingTour : EntityBase<int> , IDateTracking
    {
        public int UserId { get; set; }
        public int ScheduleId { get; set; } 
        public int Seats {  get; set; } 
<<<<<<< HEAD
        public bool IsLunch { get; set; }
		public bool IsTip {  get; set; }    
        public bool IsEntranceTicket { get; set; }
        public string Status { get; set; }    
        public double PriceTotal { get; set; }  
        [NotMapped]
        public List<Traveller>  TravellerList { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
		public DateTime? DeletedAt { get; set; }
		[Column(TypeName = "JSON")]
		public string Travellers
		{
			get => TravellerList == null ? null : JsonConvert.SerializeObject(TravellerList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						TravellerList = JsonConvert.DeserializeObject<List<Traveller>>(value) ?? new List<Traveller>();
					}
					catch (JsonException)
					{
						TravellerList = new List<Traveller>();
					}
				}
				else
				{
					TravellerList = new List<Traveller>(); 
				}
			}
		}
    }

	public class Traveller
	{
		public string Gender { get; set; }
		public string Fullname { get; set; }
		public sbyte Age { get; set; }
		public string Phone { get; set; }
	}
=======
        public bool Umbrella { get; set; }  
        public bool IsCleaningFee { get; set; } 
        public bool IsTip {  get; set; }    
        public bool IsEntranceTicket { get; set; }
        public bool Status { get; set; }    

        public double PriceTotal { get; set; }  
        public double Coupon {  get; set; } 
        public int PaymentMethod { get; set; }
        [NotMapped]
        public Traveller[] TravellerList { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
        [Column(TypeName = "JSON")]
        public string Travellers
        {
            get => JsonConvert.SerializeObject(TravellerList);
            set => TravellerList = JsonConvert.DeserializeObject<Traveller[]>(value) ?? new Traveller[0];
        }
        public ICollection<TourBookingRoom>? TourBookingRooms { get; set; }
    }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}
