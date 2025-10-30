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

        public async Task<Staff?> GetStaffByUserNameAsync(string userName)
        {
            return await dbContext.Staff
                .FirstOrDefaultAsync(s => s.UserName == userName);
        }
    }
}
