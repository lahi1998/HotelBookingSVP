using Application.Dtos.CleaningSchedule;
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

        public async Task<IEnumerable<CleaningSchedule>> GetByBookingIdWithRoomAsync(int bookingId)
        {
            return await dbContext.CleaningSchedules
                .Include(cs => cs.Room)
				.Where(cs => cs.Room!.Bookings.Any(b => b.ID == bookingId))
                .ToListAsync();
		}

		public async Task<IEnumerable<CleaningSchedule>> GetPendingWithRoomAsync()
		{
            return await dbContext.CleaningSchedules
                .Where(cs => !cs.Cleaned)
                .Include(cs => cs.Room)
                .ToListAsync();
		}

		public async Task<CleaningSchedule?> GetByIdAsync(int id)
		{
            return await dbContext.CleaningSchedules.SingleOrDefaultAsync(cs => cs.ID == id);
		}

		public async Task<CleaningSchedule> CreateForRoomAsync(CleaningSchedule cleaningSchedule)
		{
			var result = await dbContext.CleaningSchedules.AddAsync(cleaningSchedule);

			await dbContext.SaveChangesAsync();

			return result.Entity;

		}

        public async Task<CleaningSchedule> UpdateAsync(CleaningSchedule cleaningSchedule)
        {
            var result = dbContext.CleaningSchedules.Update(cleaningSchedule);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

		public async Task DeleteAsync(int id)
        {
            var cleaningScheduleToDelete = await dbContext.CleaningSchedules.SingleAsync(cs => cs.ID == id);

            dbContext.CleaningSchedules.Remove(cleaningScheduleToDelete);

            await dbContext.SaveChangesAsync();
		}
    }
}
