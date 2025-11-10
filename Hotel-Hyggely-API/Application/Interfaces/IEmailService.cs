using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Booking booking);
	}
}
