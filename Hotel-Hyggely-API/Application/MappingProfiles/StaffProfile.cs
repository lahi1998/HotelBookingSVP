using AutoMapper;

namespace Application.MappingProfiles
{
    public class StaffProfile : Profile
    {
        public StaffProfile()
        {
            CreateMap<Domain.Entities.Staff, Dtos.Staff.StaffDto>();

            CreateMap<Requests.Staff.CreateStaffRequest, Domain.Entities.Staff>();

            CreateMap<Requests.Staff.UpdateStaffRequest, Domain.Entities.Staff>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
		}
    }
}
