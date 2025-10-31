using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoomTypeRepo : IRoomTypeRepo
    {
        private readonly AppDbContext dbContext;

        public RoomTypeRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<RoomType>> GetAll()
        {
            return await dbContext.RoomTypes.ToListAsync();
		}
    }
}
