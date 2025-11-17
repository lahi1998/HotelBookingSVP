using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmailAsync(Booking booking)
        {
			var client = new SendGridClient(configuration["SendGrid:API_KEY"]);
			var from = new EmailAddress(configuration["SendGrid:email"], "Hotel Hyggely");
			var to = new EmailAddress(booking.Guest!.Email);
            var subject = "Din Booking hos Hotel Hyggely er bekræftet";

			var roomSummary = booking.Rooms
	            .GroupBy(r => r.RoomType!.Name)
	            .Select(g => $"{g.Count()} × {g.Key}")
	            .ToList();
			var roomListText = string.Join("\n- ", roomSummary);

			var plainTextContent = $@"Kære {booking.Guest!.FullName},

                Tak for din reservation hos Hotel Hyggely!

                Her er dine bookingoplysninger:
                - Dato: {booking.StartDate} til {booking.EndDate}
                - Værelser: 
                {roomListText}
                - Antal gæster: {booking.PersonCount}
                - Pris: {booking.TotalPrice:N2} DKK

                Vi glæder os til at byde dig velkommen!

                De bedste hilsner,
                Teamet bag Hotel Hyggely";

			var roomListHtml = string.Join("",
				booking.Rooms
					.GroupBy(r => r.RoomType!.Name)
					.Select(g => $"<li>{g.Count()} × {g.Key}</li>")
			);

			var htmlContent = $@"
                <html>
                  <body style='font-family:Arial,sans-serif;'>
                    <h2>Din booking hos Hotel Hyggely</h2>
                    <p>Kære {booking.Guest!.FullName},</p>
                    <p>Tak for din reservation hos <strong>Hotel Hyggely</strong>!</p>
                    <p><strong>Her er dine bookingoplysninger:</strong></p>
                    <p><strong>Dato:</strong> {booking.StartDate:dd.MM.yyyy} – {booking.EndDate:dd.MM.yyyy}</p>
                    <p><strong>Værelser:</strong></p>
                    <ul>{roomListHtml}</ul>
                    <p><strong>Antal gæster:</strong> {booking.PersonCount}<br>
                       <strong>Pris:</strong> {booking.TotalPrice:N2} DKK</p>
                    </p>
                    <p>Vi glæder os til at byde dig velkommen!</p>
                    <p>De bedste hilsner,<br>Teamet bag Hotel Hyggely</p>
                  </body>
                </html>";


			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
