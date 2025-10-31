using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
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
            var result = await dbContext.Bookings.AddAsync(booking);

            await dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            await dbContext.Bookings.SingleAsync(b => b.ID == id);

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllWithCustomerAsync()
        {
            return await dbContext.Bookings.Include(b => b.Customer).ToListAsync();
        }

        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            var result = dbContext.Bookings.Update(booking);

            await dbContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}
