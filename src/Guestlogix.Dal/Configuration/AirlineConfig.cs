using Guestlogix.Dal.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guestlogix.Dal.Configuration
{
    internal class AirlineConfig : IEntityTypeConfiguration<Airline>
    {
        /// <summary>
        /// We should add more property config
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Airline> builder)
        {
            builder.HasKey(t => t.AirlineId);
            builder.Property(t => t.TwoDigitCode).IsUnicode(false).IsRequired();

            // Add indexes for SQL DB
        }
    }
}
