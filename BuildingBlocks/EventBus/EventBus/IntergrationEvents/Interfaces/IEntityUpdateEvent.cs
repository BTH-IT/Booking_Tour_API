using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntergrationEvents.Interfaces
{
    [ExcludeFromTopology]
    public interface IEntityUpdateEvent<T> : IIntergrationEvent
    {
        T? Data { get; }
    }
}
