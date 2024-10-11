using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class GrpcSettings
    {
        public string? IdentityAddress {  get; set; }
        public string? BookingAddress { get; set; }  
        public string? RoomAddress { get; set; }
        public string? TourAddress { get; set; }

    }
}
