using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Guestlogix.Dal;
using Guestlogix.Bll;
using System.Threading.Tasks;
using System.Linq;
using Shouldly;
using Guestlogix.Bll.Domain;
using System;

namespace Guestlogix.Test
{
    [TestClass]
    public class RouteServiceTest
    {
        private static IRouteService _routeService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var options = new DbContextOptionsBuilder<GlDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_database")
                .Options;

            var dbContext = new GlDbContext(options);
            dbContext.Database.EnsureCreated();

            // Seed data when first run
            if (!dbContext.Airports.Any())
            {
                new DataSeeder().SeedData(dbContext);
            }

            _routeService = new RouteService(dbContext);
        }

        [TestMethod]
        public async Task Test_Shortest_DirectFly()
        {
            var result = await _routeService.GetShortestRoutesAsync("YYZ", "JFK");
            var path = result.Select(x => x.Origin).ToList();
            path.Add(result.Last().Destination);
            var str = string.Join(" -> ", path);
            str.ShouldBe("YYZ -> JFK");

        }

        [TestMethod]
        public async Task Test_Shortest_Four_Stops()
        {
            var result = await _routeService.GetShortestRoutesAsync("YYZ", "YVR");
            var path = result.Select(x => x.Origin).ToList();
            path.Add(result.Last().Destination);
            var str = string.Join(" -> ", path);
            str.ShouldBe("YYZ -> JFK -> LAX -> YVR");

        }

        [TestMethod]
        public async Task Test_Shortest_OriginNotFoundException()
        {
            try
            {
                var result = await _routeService.GetShortestRoutesAsync("ABC", "YVR");
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<InvalidOriginException>();
                ex.Message.ShouldBe(Messages.Invalid_Origin);
            }
        }


        [TestMethod]
        public async Task Test_Shortest_DestinationNotFoundException()
        {
            try
            {
                var result = await _routeService.GetShortestRoutesAsync("YYZ", "ABCD");
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<InvalidDestinationException>();
                ex.Message.ShouldBe(Messages.Invalid_Destination);
            }

        }

        [TestMethod]
        public async Task Test_Shortest_RouteNotFoundException()
        {
            try
            {
                var result = await _routeService.GetShortestRoutesAsync("YYZ", "ORD");
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<RouteNotFoundException>();
                ex.Message.ShouldBe(Messages.No_Route);
            }

        }

    }
}
