using System;
using System.Collections.Generic;
using System.Text;

namespace Guestlogix.Dal.Domain
{
    public class Airport
    {
        public int AirportId { get; set; }
        public string IATA3 { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
