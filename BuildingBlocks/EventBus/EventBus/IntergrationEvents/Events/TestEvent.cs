using EventBus.IntergrationEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntergrationEvents.Events
{
    public record TestEvent : IntergrationEvent, ITestEvent
    {
        public string? Hello { get ; set ; }
    }
}
