using Application.Dtos.Customer;
using Application.Dtos.Room;
using Application.Requests.Room;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType!.Name))
				.ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.RoomStatuses!.Where(r => r.StartDate <= DateTime.UtcNow && r.EndDate >= DateTime.UtcNow).FirstOrDefault().Status.ToString()));
			CreateMap<Room, RoomDetailsDto>();
            CreateMap<Room, AvailableRoomDto>();
            CreateMap<Room, CreateRoomRequest>();
            CreateMap<Room, UpdateRoomRequest>();
        }
    }
}
