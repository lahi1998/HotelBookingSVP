using Application.Requests.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
	[Authorize(Roles = "Admin, Receptionist")]
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController : ControllerBase
	{
		private readonly Application.Services.BookingService bookingService;
		public BookingsController(Application.Services.BookingService bookingService)
		{
			this.bookingService = bookingService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllBookingsAsync()
		{
			var bookings = await bookingService.GetAllBookingsAsync();

			return Ok(bookings);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetBookingAsync(int id)
		{
			var bookingDetails = await bookingService.GetByIdWithDetailsAsync(id);

			if (bookingDetails is null)
			{
				return NotFound();
			}

			return Ok(bookingDetails);
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingRequest request)
		{
			var createdBooking = await bookingService.CreateBookingAsync(request);

			return CreatedAtAction(nameof(GetBookingAsync), new { id = createdBooking.Id }, createdBooking);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateBookingAsync([FromBody] UpdateBookingRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var booking = await bookingService.UpdateBookingAsync(request);

			if (booking is null)
			{
				return NotFound();
			}

			return Ok(booking);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBookingAsync(int id)
		{
			var bookingToDelete = await bookingService.GetByIdAsync(id);

			if (bookingToDelete == null)
			{
				return NotFound();
			}

			await bookingService.DeleteBookingAsync(id);

			return NoContent();
		}

		[HttpPatch("{id}/check-in")]
		public async Task<IActionResult> CheckInAsync(int id)
		{
			var booking = await bookingService.GetByIdAsync(id);

			if (booking is null)
			{
				return NotFound();
			}

			if (booking.CheckInStatus == Domain.Enums.CheckInStatus.CheckedIn)
			{
				return Conflict("Booking is already checked in.");
			}

			if (booking.CheckInStatus == Domain.Enums.CheckInStatus.CheckedOut)
			{
				return Conflict("Booking has already been checked out.");
			}

			await bookingService.CheckInAsync(id);

			return NoContent();
		}

		[HttpPatch("{id}/check-out")]
		public async Task<IActionResult> CheckOutAsync(int id)
		{
			var booking = await bookingService.GetByIdAsync(id);

			if (booking is null)
			{
				return NotFound();
			}

			if (booking.CheckInStatus == Domain.Enums.CheckInStatus.CheckedOut)
			{
				return Conflict("Booking has already been checked out.");
			}

			if (booking.CheckInStatus == Domain.Enums.CheckInStatus.NotCheckedIn)
			{
				return Conflict("Booking has not been checked in yet.");
			}

			await bookingService.CheckOutAsync(id);

			return NoContent();
		}
	}
}
