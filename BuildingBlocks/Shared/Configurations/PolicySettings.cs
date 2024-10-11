using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class PolicySettings
    {
        public RetrySettings Retry { get; set; }
        public CircuitBreakerSettings CircuitBreaker { get; set; }
    }
    public class RetrySettings
    {
        public int RetryCount { get; set; }
        public int SleepDuration { get; set; }
    }

    public class CircuitBreakerSettings
    {
        public int RetryCount { get; set; }
        public int BreakDuration { get; set; }
    }
}
