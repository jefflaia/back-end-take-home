using System;
using System.Collections.Generic;
using System.Text;

namespace Guestlogix.Bll.Domain
{
    public class InvalidDestinationException : Exception
    {
        public InvalidDestinationException() : base(Messages.Invalid_Destination)
        { }
    }
}
