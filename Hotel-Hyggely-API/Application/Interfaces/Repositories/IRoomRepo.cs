using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomRepo
    {
		Task<IEnumerable<Room>> GetAvailableByPeriod(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Room>> GetByIdsWithRoomTypeAsync(IEnumerable<int> ids);
	}
}
