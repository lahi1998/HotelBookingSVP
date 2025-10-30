using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class RoomTypeService
    {
        private readonly IRoomTypeRepo roomTypeRepo;

        public RoomTypeService(IRoomTypeRepo roomTypeRepo)
        {
            this.roomTypeRepo = roomTypeRepo;
        }

		public async Task<IEnumerable<RoomType>> GetAllAsync()
		{
			return await roomTypeRepo.GetAll();
		}
	}
}
