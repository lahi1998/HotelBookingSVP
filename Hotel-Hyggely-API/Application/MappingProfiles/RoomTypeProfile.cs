using Application.Dtos.RoomType;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class RoomTypeProfile : Profile
    {
        public RoomTypeProfile()
        {
            CreateMap<RoomType, RoomTypeDto>();
        }
    }
}
