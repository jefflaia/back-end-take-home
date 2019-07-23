using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guestlogix.Bll.Domain;
using Guestlogix.Dal;
using Guestlogix.Dal.Domain;
using Microsoft.EntityFrameworkCore;

namespace Guestlogix.Bll
{
    /// <summary>
    /// 
    /// </summary>
    public class RouteService : IRouteService
    {
        private GlDbContext _dbContext;

        public RouteService(GlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flightRoutes"></param>
        /// <returns></returns>
        public async Task<List<Flight>> GetFlightsDetailsAsync(List<Route> flightRoutes)
        {

            var airlineIds = flightRoutes.Select(r => r.AirlineId).Distinct().ToList();
            var airportIds = flightRoutes.SelectMany(r => new string[] { r.Origin, r.Destination }).Distinct().ToList();

            var airlines = await _dbContext.Airlines
                .Where(l => airlineIds.Contains(l.TwoDigitCode))
                .Select(l => new { l.Name, l.TwoDigitCode })
                .ToListAsync();

            var airports = await _dbContext.Airports
                .Where(p => airportIds.Contains(p.IATA3))
                .Select(p => new { p.IATA3, p.Name, p.City, p.Country })
                .ToListAsync();

            var flights = (from r in flightRoutes
                           join l in airlines on r.AirlineId equals l.TwoDigitCode
                           join o in airports on r.Origin equals o.IATA3
                           join d in airports on r.Destination equals d.IATA3
                           select new Flight
                           {
                               AirlineId = r.AirlineId,
                               AirlineName = l.Name,
                               Origin = r.Origin,
                               OriginAirport = o.Name,
                               OriginCity = o.City,
                               OriginCountry = o.Country,
                               Destination = r.Destination,
                               DestinationAirport = d.Name,
                               DestinationCity = d.City,
                               DestinationCountry = d.Country,
                           }).ToList();

            return flights;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public async Task<List<Route>> GetShortestRoutesAsync(string origin, string destination)
        {
            if (!_dbContext.Airports.Any(a => a.IATA3 == origin))
            {
                throw new InvalidOriginException();
            }
            if (!_dbContext.Airports.Any(a => a.IATA3 == destination))
            {
                throw new InvalidDestinationException();
            }

            //Search route
            var orgDic = new Dictionary<string, Route>();
            var dstDic = new Dictionary<string, Route>();

            var orgQueue = new Queue<string>();
            orgQueue.Enqueue(origin);

            var dstQueue = new Queue<string>();
            dstQueue.Enqueue(destination);

            var found = false;
            var foundId = string.Empty;
            while (orgQueue.Count > 0 && dstQueue.Count > 0)
            {
                found = await SearchOneLevelAsync(orgQueue, orgDic, dstDic, destination);
                if (found)
                {
                    foundId = orgDic.Last().Key;
                    break;
                }
                found = await SearchOneLevelReverseAsync(dstQueue, orgDic, dstDic, origin);

                if (found)
                {
                    foundId = dstDic.Last().Key;
                    break;
                }
            }

            if (!found)
            {
                throw new RouteNotFoundException();
            }

            //Build flights array
            var flightRoutes = new List<Route>();
            flightRoutes.Add(orgDic[foundId]);

            while (flightRoutes.First().Origin != origin)
            {
                flightRoutes.Insert(0, orgDic[flightRoutes.First().Origin]);
            }

            while (flightRoutes.Last().Destination != destination)
            {
                flightRoutes.Add(dstDic[flightRoutes.Last().Destination]);
            }


            return flightRoutes;
        }

        /// <summary>
        /// Search from origin to destination
        /// </summary>
        /// <param name="orgQueue"></param>
        /// <param name="orgDic"></param>
        /// <param name="dstDic"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private async Task<bool> SearchOneLevelAsync(Queue<string> orgQueue, Dictionary<string, Route> orgDic, Dictionary<string, Route> dstDic, string destination)
        {
            bool found = false;
            var count = orgQueue.Count;
            for (int i = 0; i < count; i++)
            {
                var element = orgQueue.Dequeue();

                var routes = await GetRoutesByOriginAsync(element);
                foreach (var route in routes)
                {
                    if (route.Destination == destination)
                    {
                        orgDic[destination] = route;
                        orgQueue.Clear();
                        found = true;
                        break;
                    }

                    if (dstDic.ContainsKey(route.Destination))
                    {
                        orgDic[route.Destination] = route;
                        orgQueue.Clear();
                        found = true;
                        break;
                    }

                    if (!orgDic.ContainsKey(route.Destination))
                    {
                        orgDic[route.Destination] = route;
                        orgQueue.Enqueue(route.Destination);
                    }
                }

                // stop loop
                if (found)
                    break;
            }

            return found;
        }

        /// <summary>
        /// Search from destination to origin. Combination both will speed up the process and reduce the database access.
        /// </summary>
        /// <param name="dstQueue"></param>
        /// <param name="orgDic"></param>
        /// <param name="dstDic"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        private async Task<bool> SearchOneLevelReverseAsync(Queue<string> dstQueue, Dictionary<string, Route> orgDic, Dictionary<string, Route> dstDic, string origin)
        {
            bool found = false;
            var count = dstQueue.Count;
            for (int i = 0; i < count; i++)
            {
                var element = dstQueue.Dequeue();

                var routes = await GetRoutesByDestinationAsync(element);
                foreach (var route in routes)
                {
                    if (orgDic.ContainsKey(route.Origin))
                    {
                        dstDic[route.Origin] = route;
                        dstQueue.Clear();
                        found = true;
                        break;
                    }

                    if (!dstDic.ContainsKey(route.Origin))
                    {
                        dstDic[route.Origin] = route;
                        dstQueue.Enqueue(route.Origin);
                    }
                }

                // stop loop
                if (found)
                    break;
            }

            return found;
        }

        private async Task<IList<Route>> GetRoutesByOriginAsync(string origin)
        {
            var routes = await _dbContext.Routes.Where(r => r.Origin == origin).ToListAsync();

            return routes;
        }

        private async Task<IList<Route>> GetRoutesByDestinationAsync(string destination)
        {
            var routes = await _dbContext.Routes.Where(r => r.Destination == destination).ToListAsync();

            return routes;
        }


    }
}
