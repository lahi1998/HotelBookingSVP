using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICleaningScheduleRepo
    {
		Task<IEnumerable<CleaningSchedule>> GetByBookingIdWithRoom(int bookingId);
	}
}
