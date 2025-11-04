using Application.Dtos.RoomType;
using Application.Interfaces.Repositories;
using AutoMapper;

namespace Application.Services
{
    public class RoomTypeImageService
    {
        private readonly IRoomTypeImageRepo imageRepo;
        private readonly IMapper mapper;

        public RoomTypeImageService(IRoomTypeImageRepo imageRepo, IMapper mapper)
        {
            this.imageRepo = imageRepo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RoomTypeImageDto>> GetAllAsync()
        {
            var images = await imageRepo.GetAllAsync();

            return mapper.Map<IEnumerable<RoomTypeImageDto>>(images);
		}

		public async Task<IEnumerable<RoomTypeImageDto>> GetByRoomTypeIdAsync(int id)
        {
            var images = await imageRepo.GetByRoomTypeIdAsync(id);

			return mapper.Map<IEnumerable<RoomTypeImageDto>>(images);
		}
	}
}
