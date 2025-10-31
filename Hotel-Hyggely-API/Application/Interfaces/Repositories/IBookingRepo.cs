using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories
{
    public interface IBookingRepo
    {
        Task<IEnumerable<Booking>> GetAllWithCustomerAndRoomsAsync();
        Task<Booking?> GetById(int id);
		Task<Booking?> GetByIdWithDetails(int id);
		Task<Booking> CreateAsync(Booking booking);
        Task<Booking?> UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
    }
}
