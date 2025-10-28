using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations
{
    public class BookingConf : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.CheckInStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(b => b.Comment)
                .HasMaxLength(1000);
        }
    }
}
