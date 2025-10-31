using Application.Dtos.Booking;
using Application.Dtos.Room;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepo roomRepo;
        private readonly IMapper mapper;

        public RoomService(IRoomRepo roomRepo, IMapper mapper)
        {
            this.roomRepo = roomRepo;
            this.mapper = mapper;
        }

		public async Task<IEnumerable<AvailableRoomDto>> GetAvailableByPeriod(DateTime fromDate, DateTime toDate)
        {
            var availableRooms = await roomRepo.GetAvailableByPeriod(fromDate, toDate);


            return mapper.Map<IEnumerable<AvailableRoomDto>>(availableRooms);

        }
    }
}
