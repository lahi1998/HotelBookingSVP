using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IStaffRepo
    {
		Task<IEnumerable<Staff>> GetAllAsync();
		Task<Staff?> GetByUserNameAsync(string userName);
		Task<Staff?> GetByIdAsync(int id);
		Task<Staff> CreateAsync(Staff staff);
		Task<Staff?> UpdateAsync(Booking staff);
		Task DeleteAsync(int id);
	}
}
