using Application.Dtos.Customer;
using Application.Dtos.Room;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<Room, RoomDetailsDto>();
            CreateMap<Room, AvailableRoomDto>();
        }
    }
}
