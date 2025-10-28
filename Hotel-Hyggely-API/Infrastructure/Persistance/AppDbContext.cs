using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Infrastructure.Persistance.Configurations;

namespace Infrastructure.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<CleaningSchedule> CleaningSchedules { get; set; }
        public DbSet<Customer> Customers{ get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<RoomType> RoomTypes{ get; set; }
        public DbSet<Staff> Staff{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingConf).Assembly);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>()
                .HaveMaxLength(255);
        }
    }
}
