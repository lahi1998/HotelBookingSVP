using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomRepo
    {
		Task<IEnumerable<Room>> GetAllAsync();
		Task<IEnumerable<Room>> GetAvailableByPeriod(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Room>> GetByIdsWithRoomTypeAsync(IEnumerable<int> ids);
		Task<Room?> GetByIdAsync(int id);
		Task<Room> CreateAsync(Room room);
		Task<Room> UpdateAsync(Room room);
		Task DeleteAsync(int id);
	}
}
