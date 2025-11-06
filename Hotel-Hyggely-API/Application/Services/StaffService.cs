using Application.Dtos.Booking;
using Application.Dtos.Staff;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Requests.Staff;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
	public class StaffService
	{
		private readonly IStaffRepo staffRepo;
		private readonly IMapper mapper;
		private readonly IPasswordHasher passwordHasher;

		public StaffService(IStaffRepo staffRepo, IMapper mapper, IPasswordHasher passwordHasher)
		{
			this.staffRepo = staffRepo;
			this.mapper = mapper;
			this.passwordHasher = passwordHasher;
		}

		public async Task<IEnumerable<StaffDto>> GetAllAsync()
		{
			var staff = await staffRepo.GetAllAsync();

			return mapper.Map<IEnumerable<StaffDto>>(staff);
		}

		public async Task<StaffDto?> GetByIdAsync(int id)
		{
			var staff = await staffRepo.GetByIdAsync(id);

			return mapper.Map<StaffDto?>(staff);
		}

		public async Task<StaffDto> CreateAsync(CreateStaffRequest request)
		{
			var staff = mapper.Map<Domain.Entities.Staff>(request);

			staff.Password = passwordHasher.HashPassword(request.Password);

			var createdStaff = await staffRepo.CreateAsync(staff);

			return mapper.Map<StaffDto>(createdStaff);
		}

		public async Task<StaffDto?> UpdateAsync(UpdateStaffRequest request)
		{
			var existingStaff = await staffRepo.GetByIdAsync(request.Id);

			if (existingStaff is null)
			{
				return null;
			}

			// Copy request data to existing staff entity
			mapper.Map(request, existingStaff);

			// Only update password if provided and different from stored hash
			if (!string.IsNullOrWhiteSpace(request.Password) && !passwordHasher.VerifyPassword(request.Password, existingStaff.Password))
			{
				existingStaff.Password = passwordHasher.HashPassword(request.Password);
			}

			var updatedStaff = await staffRepo.UpdateAsync(existingStaff);

			return mapper.Map<StaffDto>(updatedStaff);
		}

		public async Task DeleteAsync(int id)
		{
			await staffRepo.DeleteAsync(id);
		}
	}
}
