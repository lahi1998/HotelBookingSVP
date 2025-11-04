using Application.Dtos.RoomType;
using Application.Requests.RoomType;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class RoomTypeProfile : Profile
    {
        public RoomTypeProfile()
        {
            CreateMap<RoomType, RoomTypeDto>();

            CreateMap<CreateRoomTypeRequest, RoomType>();

            CreateMap<UpdateRoomTypeRequest, RoomType>();
		}
    }
}
