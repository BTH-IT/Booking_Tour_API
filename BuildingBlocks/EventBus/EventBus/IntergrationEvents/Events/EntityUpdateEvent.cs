using EventBus.IntergrationEvents.Interfaces;

namespace EventBus.IntergrationEvents.Events
{
    public class EntityUpdateEvent<T> : IntergrationEvent, IEntityUpdateEvent<T>
    {
        public T? Data { get; set; }
    }
}
