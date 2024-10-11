using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.IntergrationEvents.Interfaces;

namespace EventBus.IntergrationEvents.Events
{
    public record IntergrationEvent() : IIntergrationEvent
    {
        public DateTime CreationDate { get; set; }
        public Guid Id { get; set; }
    }
}
