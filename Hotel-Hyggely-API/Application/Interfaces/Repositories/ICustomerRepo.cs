using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICustomerRepo
	{
		Task<Customer?> GetByEmailAsync(string email);
	}
}
