using System;

namespace Guestlogix.Bll.Domain
{
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException() : base(Messages.No_Route)
        { }
    }
}
