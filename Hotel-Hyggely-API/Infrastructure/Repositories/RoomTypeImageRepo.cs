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

        public async Task<RoomTypeImage?> GetByIdAsync(int id)
        {
            return await dbContext.RoomTypeImages.SingleOrDefaultAsync(r => r.Id == id);
		}

		public async Task<IEnumerable<RoomTypeImage>> GetByRoomTypeIdAsync(int id)
        {
            return await dbContext.RoomTypeImages.Where(r => r.Id == id).ToListAsync();
        }

        public async Task<RoomTypeImage> CreateAsync(RoomTypeImage roomTypeImage)
        {
            var result = await dbContext.RoomTypeImages.AddAsync(roomTypeImage);

            await dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var imageToDelete = await dbContext.RoomTypeImages.SingleAsync(r => r.Id == id);
            dbContext.RoomTypeImages.Remove(imageToDelete);
            await dbContext.SaveChangesAsync();
        }
    }
}
