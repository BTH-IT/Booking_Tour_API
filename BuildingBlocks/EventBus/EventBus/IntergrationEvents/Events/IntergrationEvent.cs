using EventBus.IntergrationEvents.Interfaces;

namespace EventBus.IntergrationEvents.Events
{
    public class IntergrationEvent : IIntergrationEvent
    {
        public DateTime CreationDate { get; set; }
        public Guid Id { get; set; }
        public int ObjectId { get; set; }
    }
}
