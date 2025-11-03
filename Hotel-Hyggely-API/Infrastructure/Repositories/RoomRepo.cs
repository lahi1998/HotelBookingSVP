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
    }
}
