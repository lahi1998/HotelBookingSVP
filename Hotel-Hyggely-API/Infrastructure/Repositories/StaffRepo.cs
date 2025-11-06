using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StaffRepo : IStaffRepo
    {
        private readonly AppDbContext dbContext;

        public StaffRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<IEnumerable<Staff>> GetAllAsync()
		{
			return await dbContext.Staff
				.ToListAsync();
		}

		public async Task<Staff?> GetByIdAsync(int id)
		{
			return await dbContext.Staff
				.SingleOrDefaultAsync(b => b.ID == id);
		}

		public async Task<Staff?> GetByUserNameAsync(string userName)
		{
			return await dbContext.Staff
				.SingleOrDefaultAsync(s => s.UserName == userName);
		}

		public async Task<Staff> CreateAsync(Staff staff)
        {
			var result = await dbContext.Staff.AddAsync(staff);

			await dbContext.SaveChangesAsync();

			return result.Entity;
		}

        public async Task<Staff> UpdateAsync(Staff staff)
        {
			var result = dbContext.Staff.Update(staff);

			await dbContext.SaveChangesAsync();

			return result.Entity;
		}
        public async Task DeleteAsync(int id)
        {
			var staffToDelete = await dbContext.Staff.SingleAsync(b => b.ID == id);

			dbContext.Staff.Remove(staffToDelete);

			await dbContext.SaveChangesAsync();
		}
    }
}
