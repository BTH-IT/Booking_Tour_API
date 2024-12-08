using EventBus.IntergrationEvents.Interfaces;

namespace EventBus.IntergrationEvents.Events
{
    public class EntityEvent<T> : IntergrationEvent, IEntityAddEvent<T>
    {
        public T? Data { get; set; }
        public string Type { get; set; }
    }
}
