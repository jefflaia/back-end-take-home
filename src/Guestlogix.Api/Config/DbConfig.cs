using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guestlogix.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Guestlogix.Api.Config
{
    public class DbConfig
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GlDbContext(serviceProvider.GetRequiredService<DbContextOptions<GlDbContext>>()))
            {
                context.Database.EnsureCreated();

                // Seed data when first run
                if (!context.Airports.Any())
                {
                    new DataSeeder().SeedData(context);
                }
            }
        }
    }

}
