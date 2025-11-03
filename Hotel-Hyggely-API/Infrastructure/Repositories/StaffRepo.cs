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

		public Task<IEnumerable<Staff>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Staff?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<Staff?> GetByUserNameAsync(string userName)
		{
			return await dbContext.Staff
				.FirstOrDefaultAsync(s => s.UserName == userName);
		}

		public Task<Staff> CreateAsync(Staff staff)
        {
            throw new NotImplementedException();
        }

        public Task<Staff?> UpdateAsync(Booking staff)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
