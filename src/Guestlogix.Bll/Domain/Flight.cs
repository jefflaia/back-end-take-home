using System;
using System.Collections.Generic;
using System.Text;

namespace Guestlogix.Bll.Domain
{
    public class Flight
    {
        public string AirlineId { get; set; }
        public string AirlineName { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }

        public string OriginAirport { get; set; }
        public string OriginCity { get; set; }
        public string OriginCountry { get; set; }
        public string DestinationAirport { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationCountry { get; set; }

    }
}
