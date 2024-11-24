﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntergrationEvents.Interfaces
{
    public interface IEntityAddEvent<T> : IIntergrationEvent
    {
        T? Data { get; }
        string Type { get; }


    }
}