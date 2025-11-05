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
				.ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src =>
				src.MaintenancePeriods.Any(m =>
					m.StartDate <= DateTime.Now &&
					m.EndDate >= DateTime.Now)
					? "Under Vedligeholdelse"
					: src.Bookings.Any(b =>
						b.StartDate <= DateTime.Now &&
						b.EndDate >= DateTime.Now)
						? "Reserveret"
						: "Ledigt"
				));

			CreateMap<Room, RoomDetailsDto>();
			CreateMap<Room, AvailableRoomDto>();
			CreateMap<CreateRoomRequest, Room>();
			CreateMap<UpdateRoomRequest, Room>();
		}
	}
}
