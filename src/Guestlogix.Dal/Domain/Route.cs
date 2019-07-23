using System;
using System.Collections.Generic;
using System.Text;

namespace Guestlogix.Dal.Domain
{
    public class Route
    {
        public int RouteId { get; set; }
        public string AirlineId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }

    }
}
