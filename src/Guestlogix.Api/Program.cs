using System;

using Guestlogix.Api.Config;
using Guestlogix.Dal;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog.Web;

namespace Guestlogix.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

                //1. Get the IWebHost which will host this application.
                var host = CreateWebHostBuilder(args).Build();

                //2. Find the service layer within our scope.
                using (var scope = host.Services.CreateScope())
                {
                    //3. Get the instance of DBContext in our services layer
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<GlDbContext>();

                    //4. Call the DataGenerator to create sample data
                    DbConfig.Initialize(services);
                }

                //Continue to run the application
                host.Run();

            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();  // NLog: setup NLog for Dependency injection

    }


}
