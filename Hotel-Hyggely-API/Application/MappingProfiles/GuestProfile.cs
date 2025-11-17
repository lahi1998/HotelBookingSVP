using Application.Dtos.Customer;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class GuestProfile : Profile
    {
        public GuestProfile()
        {
            CreateMap<Guest, GuestDto>();
        }
    }
}
