using Application.Dtos.Customer;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using System.Reflection.Metadata;

namespace Application.Services
{
    public class GuestService
    {
        private readonly IGuestRepo guestRepo;
        private readonly IMapper mapper;

        public GuestService(IGuestRepo guestRepo, IMapper mapper)
        {
            this.guestRepo = guestRepo;
            this.mapper = mapper;
        }

		public async Task<IEnumerable<GuestDto>> GetAllAsync()
		{
			var guests = await guestRepo.GetAllAsync();

			return mapper.Map<IEnumerable<GuestDto>>(guests);

		}

		public async Task DeleteAsync(int id)
		{
			await guestRepo.DeleteAsync(id);
		}
	}
}
