using Application.Exeptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepo : IBookingRepo
    {
        private readonly AppDbContext dbContext;

        public BookingRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<Booking?> GetById(int id)
		{
			return await dbContext.Bookings.SingleOrDefaultAsync(b => b.ID == id);
		}

		public async Task<Booking?> GetByIdWithRooms(int id)
		{
			return await dbContext.Bookings
                .Include(b => b.Rooms)
                    .ThenInclude(r => r.CleaningSchedules)
				.SingleOrDefaultAsync(b => b.ID == id);
		}
		public async Task<Booking?> GetByIdWithDetails(int id)
		{
			return await dbContext.Bookings
                .Include(b => b.Customer)
				.Include(b => b.Rooms)
                    .ThenInclude(r => r.RoomType)
				.SingleOrDefaultAsync(b => b.ID == id);
		}

		public async Task<Booking> CreateAsync(Booking booking)
        {
			await using var transaction = await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

			var roomIds = booking.Rooms.Select(r => r.ID).ToList();
			var startDate = booking.StartDate;
			var endDate = booking.EndDate;

			// Check if any of the rooms are already booked or in maintenance
			var conflictExists = await dbContext.Rooms
				.Where(r => roomIds.Contains(r.ID))
				.AnyAsync(r =>
					r.Bookings.Any(b => b.StartDate < endDate && startDate < b.EndDate) ||
					r.MaintenancePeriods.Any(m => m.StartDate < endDate && startDate < m.EndDate)
				);

			if (conflictExists)
				throw new BookingConflictExeption("Room is already booked in the selected date range.");

			// Add the booking
			var result = await dbContext.Bookings.AddAsync(booking);
			await dbContext.SaveChangesAsync();

			// Commit the transaction
			await transaction.CommitAsync();

			return result.Entity;
		}

        public async Task DeleteAsync(int id)
        {
            var bookingToDelete = await dbContext.Bookings.SingleAsync(b => b.ID == id);

            dbContext.Bookings.Remove(bookingToDelete);

			await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllWithCustomerAndRoomsAsync()
        {
            return await dbContext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Rooms)
                .ToListAsync();
        }

        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            var result = dbContext.Bookings.Update(booking);

            await dbContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}
