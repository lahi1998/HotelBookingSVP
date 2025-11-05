using Application.Dtos.CleaningSchedule;
using Application.Interfaces.Repositories;
using Application.Requests.CleaningSchedule;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class CleaningScheduleService
    {
        private readonly ICleaningScheduleRepo cleaningScheduleRepo;
        private readonly IMapper mapper;

        public CleaningScheduleService(ICleaningScheduleRepo cleaningScheduleRepo, IMapper mapper)
        {
            this.cleaningScheduleRepo = cleaningScheduleRepo;
            this.mapper = mapper;
        }

		public async Task<IEnumerable<CleaningScheduleDto>> GetByBookingIdAsync(int bookingId)
		{
			var cleaningSchedules = await cleaningScheduleRepo.GetByBookingIdWithRoomAsync(bookingId);

			return mapper.Map<IEnumerable<CleaningScheduleDto>>(cleaningSchedules);
		}

		public async Task<IEnumerable<CleaningScheduleDto>> GetPendingWithRoomAsync()
        {
            var cleaningSchedules = await cleaningScheduleRepo.GetPendingWithRoomAsync();

            return mapper.Map<IEnumerable<CleaningScheduleDto>>(cleaningSchedules);
		}

		public async Task<CleaningScheduleDto?> GetByIdAsync(int id)
        {
            var cleaningSchedule = await cleaningScheduleRepo.GetByIdAsync(id);

			return mapper.Map<CleaningScheduleDto?>(cleaningSchedule);
		}

		public async Task<CleaningScheduleDto> CreateForRoomAsync(CreateCleaningScheduleRequest request)
        {
            var cleaningSchedule = mapper.Map<CleaningSchedule>(request);

			var createdCleaningSchedule = await cleaningScheduleRepo.CreateForRoomAsync(cleaningSchedule);

            return mapper.Map<CleaningScheduleDto>(createdCleaningSchedule);
		}

        public async Task<bool> MarkAsCleaned(int id)
        {
            var existingCleaningSchedule = await cleaningScheduleRepo.GetByIdAsync(id);

            if (existingCleaningSchedule == null)
            {
                return false;
            }

            existingCleaningSchedule.Room!.LastCleanedDate = DateTime.Now;
			existingCleaningSchedule.Cleaned = true;
            await cleaningScheduleRepo.UpdateAsync(existingCleaningSchedule);
            return true;
        }

		public async Task<bool> DeleteAsync(int id)
        {
            var existingCleaningSchedule = await cleaningScheduleRepo.GetByIdAsync(id);

            if (existingCleaningSchedule == null)
            {
                return false;
			}

            await cleaningScheduleRepo.DeleteAsync(id);

            return true;
		}
	}
}
