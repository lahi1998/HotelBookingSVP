using Application.Dtos.Customer;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}
