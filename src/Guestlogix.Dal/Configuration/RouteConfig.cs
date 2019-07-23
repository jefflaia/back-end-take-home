using Guestlogix.Dal.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guestlogix.Dal.Configuration
{
    internal class RouteConfig : IEntityTypeConfiguration<Route>
    {
        /// <summary>
        /// We should add more property config
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(t => t.RouteId);
            builder.Property(t => t.AirlineId).IsUnicode(false).IsRequired();
            builder.Property(t => t.Origin).IsUnicode(false).IsRequired();
            builder.Property(t => t.Destination).IsUnicode(false).IsRequired();

            // Add indexes for SQL DB

        }
    }
}
