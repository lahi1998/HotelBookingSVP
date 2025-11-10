using Application.Dtos.Booking;
using Application.Dtos.Room;
using Application.Dtos.Staff;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Requests.Room;
using Application.Requests.Staff;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepo roomRepo;
        private readonly IMapper mapper;
        private readonly IRoomTypeRepo roomTypeRepo;

        public RoomService(IRoomRepo roomRepo, IMapper mapper, IRoomTypeRepo roomTypeRepo)
        {
            this.roomRepo = roomRepo;
            this.mapper = mapper;
            this.roomTypeRepo = roomTypeRepo;
        }

		public async Task<IEnumerable<RoomDto>> GetAllAsync()
		{
			var rooms = await roomRepo.GetAllAsync();

			return mapper.Map<IEnumerable<RoomDto>>(rooms);
		}

		public async Task<IEnumerable<AvailableRoomDto>> GetAvailableByPeriod(DateTime fromDate, DateTime toDate)
        {
            var availableRooms = await roomRepo.GetAvailableByPeriod(fromDate, toDate);

            return mapper.Map<IEnumerable<AvailableRoomDto>>(availableRooms);
        }

		public async Task<IEnumerable<RoomDto>> GetAvailableByPeriodWithDetails(DateTime fromDate, DateTime toDate)
		{
			var availableRooms = await roomRepo.GetAvailableByPeriod(fromDate, toDate);

			return mapper.Map<IEnumerable<RoomDto>>(availableRooms);
		}

		public async Task<RoomDto?> GetByIdAsync(int id)
		{
			var room = await roomRepo.GetByIdAsync(id);

			return mapper.Map<RoomDto?>(room);
		}

		public async Task<RoomDto> CreateAsync(CreateRoomRequest request)
		{
			var room = mapper.Map<Room>(request);

			var roomType = await roomTypeRepo.GetByNameAsync(request.RoomTypeName);
			room.RoomTypeId = roomType!.ID;

			var createdRoom = await roomRepo.CreateAsync(room);

			return mapper.Map<RoomDto>(createdRoom);
		}

		public async Task<RoomDto?> UpdateAsync(UpdateRoomRequest request)
		{
			var existingRoom = await roomRepo.GetByIdAsync(request.Id);

			if (existingRoom == null)
			{
				return null;
			}

			mapper.Map(request, existingRoom);

			var roomType = await roomTypeRepo.GetByNameAsync(request.RoomTypeName);
			existingRoom.RoomTypeId = roomType!.ID;

			var updatedRoom = await roomRepo.UpdateAsync(existingRoom);

			return mapper.Map<RoomDto>(updatedRoom);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var existingRoom = await roomRepo.GetByIdAsync(id);

			if (existingRoom == null)
			{
				return false;
			}

			await roomRepo.DeleteAsync(id);

			return true;
		}
	}
}
