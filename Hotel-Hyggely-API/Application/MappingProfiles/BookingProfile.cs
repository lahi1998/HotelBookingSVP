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
                .ForMember(dest => dest.RoomCount, opt => opt.MapFrom(src => src.Rooms.Count));

            CreateMap<Booking, BookingDetailsDto>();

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.RoomIds, opt => opt.MapFrom(src => src.Rooms.Select(r => r.ID).ToList()));


            CreateMap<CreateBookingRequest, Booking>()
                // Customer handled manually
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                // Rooms handled manually
                .ForMember(dest => dest.Rooms, opt => opt.Ignore())
                // Map primitive fields
                .ForMember(dest => dest.CheckInStatus, opt => opt.MapFrom(src => CheckInStatus.NotCheckedIn));

            CreateMap<UpdateBookingRequest, Booking>()
                // Customer handled manually
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                // Rooms handled manually
                .ForMember(dest => dest.Rooms, opt => opt.Ignore());
		}
    }
}
