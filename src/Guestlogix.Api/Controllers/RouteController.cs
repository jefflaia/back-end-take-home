using System;
using System.Threading.Tasks;

using Guestlogix.Api.Extensions;
using Guestlogix.Bll;
using Guestlogix.Bll.Domain;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace Guestlogix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [LogActionFilter]
    public class RouteController : ControllerBase
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private IRouteService _routeService;
        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        [Route("shortest/{origin}/{destination}")]
        [ProducesResponseType(200, Type = (typeof(string)))]
        public async Task<ActionResult> GetShortestRoute(string origin, string destination)
        {
            try
            {
                var routes = await _routeService.GetShortestRoutesAsync(origin.ToUpper(), destination.ToUpper());

                var flights = await _routeService.GetFlightsDetailsAsync(routes);

                return Ok(flights);
            }
            catch (Exception e)
            {
                if (e is InvalidOriginException || e is InvalidDestinationException || e is RouteNotFoundException)
                {
                    _logger.Warn($"{nameof(GetShortestRoute)} from origin {origin} to destination {destination}: {e.Message}");
                    return NotFound(e.Message);
                }
                else
                {
                    _logger.Error($"{nameof(GetShortestRoute)} from origin {origin} to destination {destination}: {e.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

        }

        [HttpGet]
        public string Index()
        {
            return @"Usage: api/Route/shortest/origin/destination. Example: api/Route/shortest/YYZ/JFK";
        }
    }
}