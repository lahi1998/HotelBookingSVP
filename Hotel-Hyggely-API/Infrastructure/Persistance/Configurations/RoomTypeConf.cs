using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations
{
    public class RoomTypeConf : IEntityTypeConfiguration<RoomType>
	{
		public void Configure(EntityTypeBuilder<RoomType> builder)
		{
			builder.HasIndex(r => r.Name)
				.IsUnique();
		}
	}
}
