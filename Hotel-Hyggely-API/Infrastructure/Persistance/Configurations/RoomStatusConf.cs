using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Configurations
{
    public class RoomStatusConf : IEntityTypeConfiguration<RoomStatus>
    {
        public void Configure(EntityTypeBuilder<RoomStatus> builder)
        {
            builder.Property(b => b.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
