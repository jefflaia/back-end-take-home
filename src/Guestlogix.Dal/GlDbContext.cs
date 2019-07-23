using Guestlogix.Dal.Configuration;
using Guestlogix.Dal.Domain;
using Microsoft.EntityFrameworkCore;

namespace Guestlogix.Dal
{
    public class GlDbContext : DbContext
    {
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Route> Routes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AirportConfig());
            modelBuilder.ApplyConfiguration(new AirlineConfig());
            modelBuilder.ApplyConfiguration(new RouteConfig());

            base.OnModelCreating(modelBuilder);

        }


        public GlDbContext(DbContextOptions<GlDbContext> options)
        : base(options)
        {
        }

    }
}
