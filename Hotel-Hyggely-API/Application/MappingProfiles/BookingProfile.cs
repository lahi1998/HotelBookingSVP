using Application.Dtos.Booking;
using Application.Dtos.Customer;
using Application.Requests.Booking;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.MappingProfiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingListItemDto>()
                .ForMember(dest => dest.RoomCount, opt => opt.MapFrom(src => src.Rooms.Count))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Guest));


			CreateMap<Booking, BookingDetailsDto>()
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Guest));

			CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.RoomIds, opt => opt.MapFrom(src => src.Rooms.Select(r => r.ID).ToList()))
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Guest));


			CreateMap<CreateBookingRequest, Booking>()
                // Customer handled manually
                .ForMember(dest => dest.Guest, opt => opt.Ignore())
                .ForMember(dest => dest.GuestId, opt => opt.Ignore())
                // Rooms handled manually
                .ForMember(dest => dest.Rooms, opt => opt.Ignore())
                // Map primitive fields
                .ForMember(dest => dest.CheckInStatus, opt => opt.MapFrom(src => CheckInStatus.NotCheckedIn));

            CreateMap<UpdateBookingRequest, Booking>()
                // Customer handled manually
                .ForMember(dest => dest.Guest, opt => opt.Ignore())
                .ForMember(dest => dest.GuestId, opt => opt.Ignore())
                // Rooms handled manually
                .ForMember(dest => dest.Rooms, opt => opt.Ignore());
		}
    }
}
