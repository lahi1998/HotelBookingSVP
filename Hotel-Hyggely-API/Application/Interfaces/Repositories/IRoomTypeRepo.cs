using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomTypeRepo
	{
        Task<IEnumerable<RoomType>> GetAll();
		Task<RoomType?> GetByIdAsync(int id);
		Task<RoomType?> GetByNameAsync(string name);
		Task<RoomType> CreateAsync(RoomType staff);
		Task<RoomType> UpdateAsync(RoomType staff);
		Task DeleteAsync(int id);
	}
}
