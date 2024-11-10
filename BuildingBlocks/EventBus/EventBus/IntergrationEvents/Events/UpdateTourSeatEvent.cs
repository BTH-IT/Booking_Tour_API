namespace EventBus.IntergrationEvents.Events
{
    public class UpdateTourSeatEvent : IntergrationEvent
    {
        public string TourId { get; set; } = string.Empty;
        public int Seats { get; set; }  
    }
}
