using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepo roomRepo;

        public RoomService(IRoomRepo roomRepo)
        {
            this.roomRepo = roomRepo;
        }

		public async Task<IEnumerable<Room>> GetByPeriod(DateTime fromDate, DateTime toDate)
        {
            return await roomRepo.GetByPeriod(fromDate, toDate);
		}
	}
}
