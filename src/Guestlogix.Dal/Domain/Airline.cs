using System;
using System.Collections.Generic;
using System.Text;

namespace Guestlogix.Dal.Domain
{
    public class Airline
    {
        public int AirlineId { get; set; }
        public string ThreeDigitCode { get; set; }
        public string TwoDigitCode { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
