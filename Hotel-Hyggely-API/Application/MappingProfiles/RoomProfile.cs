using Application.Dtos.Customer;
using Application.Dtos.Room;
using Application.Requests.Room;
using AutoMapper;
using Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
					? "Under Maintenance"
					: src.Bookings.Any(b =>
						b.StartDate <= DateTime.Now &&
						b.EndDate >= DateTime.Now)
						? "Reserved"
						: "Available"
				));


			//!r.Bookings.Any(b =>
			//			b.StartDate < toDate &&
			//			b.EndDate > fromDate)
			//		&&
			//		!r.MaintenancePeriods.Any(m =>
			//			m.StartDate < toDate &&
			//			m.EndDate > fromDate))));

			//         .Where(r =>
			//		!r.Bookings.Any(b =>
			//			b.StartDate < toDate &&
			//			b.EndDate > fromDate)
			//		&&
			//		!r.MaintenancePeriods.Any(m =>
			//			m.StartDate < toDate &&
			//			m.EndDate > fromDate))
			CreateMap<Room, RoomDetailsDto>();
            CreateMap<Room, AvailableRoomDto>();
            CreateMap<Room, CreateRoomRequest>();
            CreateMap<Room, UpdateRoomRequest>();


        }
    }
}
