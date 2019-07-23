using System;
using System.Collections.Generic;
using System.Text;

namespace Guestlogix.Bll.Domain
{
    public class InvalidOriginException : Exception
    {
        public InvalidOriginException() : base(Messages.Invalid_Origin)
        { }
    }
}
