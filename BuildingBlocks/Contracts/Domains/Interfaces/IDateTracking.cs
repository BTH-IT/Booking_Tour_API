using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domains.Interfaces
{
    public interface IDateTracking
    {
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; } 
    }
}
