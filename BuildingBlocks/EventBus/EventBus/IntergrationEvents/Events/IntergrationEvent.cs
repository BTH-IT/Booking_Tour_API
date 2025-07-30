using EventBus.IntergrationEvents.Interfaces;
using MassTransit;

namespace EventBus.IntergrationEvents.Events
{
    [ExcludeFromTopology]
    public class IntergrationEvent : IIntergrationEvent
    {
        public DateTime CreationDate { get; set; }
        public Guid Id { get; set; }
        public int ObjectId { get; set; }
    }
}
