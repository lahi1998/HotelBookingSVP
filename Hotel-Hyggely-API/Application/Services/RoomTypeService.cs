using Application.Dtos.RoomType;
using Application.Interfaces.Repositories;
using Application.Requests.RoomType;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class RoomTypeService
    {
        private readonly IRoomTypeRepo roomTypeRepo;
        private readonly IMapper mapper;

        public RoomTypeService(IRoomTypeRepo roomTypeRepo, IMapper mapper)
        {
            this.roomTypeRepo = roomTypeRepo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RoomTypeDto>> GetAllAsync()
        {
            var roomTypes = await roomTypeRepo.GetAll();

            return mapper.Map<IEnumerable<RoomTypeDto>>(roomTypes);
        }

        public async Task<RoomTypeDto?> GetByIdAsync(int id)
        {
            var roomType = await roomTypeRepo.GetByIdAsync(id);

            return mapper.Map<RoomTypeDto?>(roomType);
        }

        public async Task<RoomTypeDto> CreateAsync(CreateRoomTypeRequest request)
        {
            var roomType = mapper.Map<RoomType>(request);

            var createdRoomType = await roomTypeRepo.CreateAsync(roomType);

            return mapper.Map<RoomTypeDto>(createdRoomType);
        }

        public async Task<RoomTypeDto?> UpdateAsync(UpdateRoomTypeRequest request)
        {
            var existingRoomType = await roomTypeRepo.GetByIdAsync(request.Id);

            if (existingRoomType is null)
            {
                return null;
            }

            mapper.Map(request, existingRoomType);

            var updatedRoomType = await roomTypeRepo.UpdateAsync(existingRoomType);

            return mapper.Map<RoomTypeDto>(updatedRoomType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingRoomType = await roomTypeRepo.GetByIdAsync(id);

            if (existingRoomType is null)
            {
                return false;
            }

            await roomTypeRepo.DeleteAsync(id);

            return true;
        }
    }
}
