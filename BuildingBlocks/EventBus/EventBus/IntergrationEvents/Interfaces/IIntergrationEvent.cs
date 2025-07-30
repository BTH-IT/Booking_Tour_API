
using MassTransit;

namespace EventBus.IntergrationEvents.Interfaces
{
    [ExcludeFromTopology]
    public interface IIntergrationEvent
    {
        public DateTime CreationDate { get; set; }
        public Guid Id { get; set; }
        public int ObjectId { get; set; }

    }
}
