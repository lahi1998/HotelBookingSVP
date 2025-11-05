using Application.Dtos.CleaningSchedule;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class CleaningScheduleProfile : Profile
    {
        public CleaningScheduleProfile()
        {
            CreateMap<CleaningSchedule, CleaningScheduleDto>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room!.Number))
                .ForMember(dest => dest.RoomFloor, opt => opt.MapFrom(src => src.Room!.Floor));
		}
    }
}
