using EventBus.IntergrationEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntergrationEvents.Events
{
    public class EntityUpdateEvent<T> : IntergrationEvent, IEntityUpdateEvent<T>
    {
        public T? Data { get; set; }
    }
}
