using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoomTypeImageRepo : IRoomTypeImageRepo
    {
        private readonly AppDbContext dbContext;

        public RoomTypeImageRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<RoomTypeImage>> GetAllAsync()
        {
            return await dbContext.RoomTypeImages.ToListAsync();
		}

        public async Task<IEnumerable<RoomTypeImage>> GetByRoomTypeIdAsync(int id)
		{
            return await dbContext.RoomTypeImages.Where(r => r.Id == id).ToListAsync();
		}
    }
}
