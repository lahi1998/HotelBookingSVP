using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GuestRepo : IGuestRepo
    {
        private readonly AppDbContext dbContext;

        public GuestRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<IEnumerable<Guest>> GetAllAsync()
		{
			return await dbContext.Guests
			.ToListAsync();

		}

		public async Task<Guest?> GetByEmailAsync(string email)
		{
			return await dbContext.Guests
				.FirstOrDefaultAsync(s => s.Email == email);
		}

		public async Task DeleteAsync(int id)
        {
			var guestToDelete = await dbContext.Guests.SingleAsync(b => b.ID == id);

			dbContext.Guests.Remove(guestToDelete);

			await dbContext.SaveChangesAsync();
		}
    }
}
