using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoomRepo : IRoomRepo
    {
        private readonly AppDbContext dbContext;

        public RoomRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<IEnumerable<Room>> GetAllAsync()
		{
			return await dbContext.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomStatuses)
                .ToListAsync();
		}

		public async Task<Room?> GetByIdAsync(int id)
		{
            return await dbContext.Rooms.SingleOrDefaultAsync(r => r.ID == id);
		}

		public async Task<IEnumerable<Room>> GetByIdsWithRoomTypeAsync(IEnumerable<int> ids)
        {
            var rooms = await dbContext.Rooms
                .Include(r => r.RoomType)
				.Where(r => ids.Contains(r.ID))
                .ToListAsync();

            return rooms;
        }

        public async Task<IEnumerable<Room>> GetAvailableByPeriod(DateTime fromDate, DateTime toDate)
        {
            var rooms = await dbContext.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.RoomStatuses.Any(rs =>
                    rs.Status == RoomStatusType.Available
                    && rs.StartDate <= fromDate
                    && rs.EndDate >= toDate))
                .Distinct()
                .ToListAsync();

            return rooms;
        }

        public async Task<Room> CreateAsync(Room room)
        {
            var result = await dbContext.Rooms.AddAsync(room);

            await dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Room> UpdateAsync(Room room)
        {
			var result = dbContext.Rooms.Update(room);

			await dbContext.SaveChangesAsync();

			return result.Entity;

		}

		public async Task DeleteAsync(int id)
        {
            var roomToDelete = await dbContext.Rooms.SingleAsync(r => r.ID == id);

            dbContext.Rooms.Remove(roomToDelete);

            await dbContext.SaveChangesAsync();
        }
    }
}
