using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IGuestRepo
    {
		Task<IEnumerable<Guest>> GetAllAsync();
		Task<Guest?> GetByEmailAsync(string email);
		Task DeleteAsync(int id);
	}
}
