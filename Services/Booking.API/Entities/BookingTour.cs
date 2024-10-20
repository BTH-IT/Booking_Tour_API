using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
    
    [Table("BookingTours")]
    public class BookingTour : EntityBase<int> , IDateTracking
    {
        public int UserId { get; set; }
        public int ScheduleId { get; set; } 
        public int Seats {  get; set; } 
        public bool Umbrella { get; set; }  
        public bool IsCleaningFee { get; set; } 
        public bool IsTip {  get; set; }    
        public bool IsEntranceTicket { get; set; }
        public bool Status { get; set; }    
        public double PriceTotal { get; set; }  
        public double Coupon {  get; set; } 
		public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public int PaymentMethod { get; set; }
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
		public ICollection<TourBookingRoom> TourBookingRooms { get; set; }
    }

	public class Traveller
	{
		public bool Gender { get; set; }
		public string Fullname { get; set; }
		public sbyte Age { get; set; }
		public string Phone { get; set; }
	}
}
