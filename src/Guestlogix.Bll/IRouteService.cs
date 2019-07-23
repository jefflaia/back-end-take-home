using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Guestlogix.Bll.Domain;
using Guestlogix.Dal.Domain;

namespace Guestlogix.Bll
{
    public interface IRouteService
    {
        Task<List<Flight>> GetFlightsDetailsAsync(List<Route> flightRoutes);
        Task<List<Route>> GetShortestRoutesAsync(string origin, string destination);

    }
}
