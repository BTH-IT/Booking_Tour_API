﻿using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Entities
{
    public class Traveller
    {
        public bool Gender { get; set; }
        public string Fullname { get; set; }
        public sbyte Age { get; set; }
        public string Phone { get; set; }
    }
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
}
