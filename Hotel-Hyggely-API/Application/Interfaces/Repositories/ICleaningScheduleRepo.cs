using Application.Dtos.CleaningSchedule;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICleaningScheduleRepo
    {
        Task<IEnumerable<CleaningSchedule>> GetPendingWithRoomAsync();
        Task<CleaningSchedule?> GetByIdAsync(int id);
		Task<CleaningSchedule?> GetByIdWithRoomAsync(int id);
		Task<IEnumerable<CleaningSchedule>> GetByBookingIdWithRoomAsync(int bookingId);
        Task<CleaningSchedule> CreateForRoomAsync(CleaningSchedule cleaningSchedule);
        Task<CleaningSchedule> UpdateAsync(CleaningSchedule cleaningSchedule);
		Task DeleteAsync(int id);
    }
}
