using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IStaffRepo
    {
        Task<Staff?> GetStaffByUserNameAsync(string userName);
    }
}
