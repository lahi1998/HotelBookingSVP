using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomTypeRepo
	{
        Task<IEnumerable<RoomType>> GetAll();
    }
}
