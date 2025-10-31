using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomRepo
    {
		Task<IEnumerable<Room>> GetByPeriod(DateTime startDate, DateTime endDate);
	}
}
