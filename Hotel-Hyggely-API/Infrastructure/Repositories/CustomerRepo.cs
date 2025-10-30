using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext dbContext;

        public CustomerRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Customer?> GetByEmailAsync(string email)
        {
			return await dbContext.Customers
	            .FirstOrDefaultAsync(s => s.Email == email);
		}
	}
}
