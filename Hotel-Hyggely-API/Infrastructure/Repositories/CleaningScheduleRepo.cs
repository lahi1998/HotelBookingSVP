using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CleaningScheduleRepo : ICleaningScheduleRepo
    {
        private readonly AppDbContext dbContext;

        public CleaningScheduleRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<CleaningSchedule>> GetByBookingIdWithRoom(int bookingId)
        {
            return await dbContext.CleaningSchedules
                .Include(cs => cs.Room)
				.Where(cs => cs.Room!.Bookings.Any(b => b.ID == bookingId))
                .ToListAsync();
		}
    }
}
