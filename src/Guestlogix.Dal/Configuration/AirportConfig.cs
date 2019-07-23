using Guestlogix.Dal.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guestlogix.Dal.Configuration
{
    internal class AirportConfig : IEntityTypeConfiguration<Airport>
    {
        /// <summary>
        /// We should add more property config
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.HasKey(t => t.AirportId);
            builder.Property(t => t.IATA3).IsUnicode(false).IsRequired();

            // Add indexes for SQL DB

        }
    }
}
