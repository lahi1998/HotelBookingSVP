using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomTypeImageRepo
    {
		Task<IEnumerable<RoomTypeImage>> GetAllAsync();
		Task<IEnumerable<RoomTypeImage>> GetByRoomTypeIdAsync(int id);
	}
}
