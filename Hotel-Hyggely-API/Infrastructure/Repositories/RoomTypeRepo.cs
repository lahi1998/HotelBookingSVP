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

        public async Task<RoomType?> GetByIdAsync(int id)
        {
            return await dbContext.RoomTypes.SingleOrDefaultAsync(rt => rt.ID == id);
		}

        public async Task<RoomType> CreateAsync(RoomType roomType)
        {
            var result = dbContext.RoomTypes.AddAsync(roomType);

            await dbContext.SaveChangesAsync();

            return result.Result.Entity;
		}

        public async Task<RoomType> UpdateAsync(RoomType roomType)
        {
            var result = dbContext.RoomTypes.Update(roomType);

            await dbContext.SaveChangesAsync();

            return result.Entity;
		}

        public async Task DeleteAsync(int id)
        {
            var roomTypeToDelete = dbContext.RoomTypes.SingleAsync(rt => rt.ID == id);

            dbContext.RoomTypes.Remove(roomTypeToDelete.Result);

            await dbContext.SaveChangesAsync();
		}
    }
}
