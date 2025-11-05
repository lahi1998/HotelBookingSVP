using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomTypeImageRepo
    {
		Task<IEnumerable<RoomTypeImage>> GetAllAsync();
		Task<RoomTypeImage?> GetByIdAsync(int id);
		Task<IEnumerable<RoomTypeImage>> GetByRoomTypeIdAsync(int id);
		Task<RoomTypeImage> CreateAsync(RoomTypeImage roomTypeImage);
		Task DeleteAsync(int id);
	}
}
