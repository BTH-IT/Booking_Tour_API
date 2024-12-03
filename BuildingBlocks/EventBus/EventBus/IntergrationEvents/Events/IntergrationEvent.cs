<<<<<<< HEAD
﻿using EventBus.IntergrationEvents.Interfaces;

namespace EventBus.IntergrationEvents.Events
{
    public class IntergrationEvent : IIntergrationEvent
    {
        public DateTime CreationDate { get; set; }
        public Guid Id { get; set; }
        public int ObjectId { get; set; }
=======
﻿using System;
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
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    }
}
