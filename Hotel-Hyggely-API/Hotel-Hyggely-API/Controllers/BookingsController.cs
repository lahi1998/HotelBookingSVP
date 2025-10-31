using Application.Dtos;
using Application.Dtos.Booking;
using Application.Dtos.Customer;
using Application.Dtos.Room;
using Application.Dtos.RoomType;
using Application.Requests.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly Application.Services.BookingService bookingService;
        public BookingsController(Application.Services.BookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var bookings = await bookingService.GetAllBookingsAsync();

			return Ok(bookings);
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetBookingAsync(int id)
		{
			var bookingDetails = await bookingService.GetByIdWithDetailsAsync(id);

            if(bookingDetails is null)
            {
                return NotFound();
            }

			return Ok(bookingDetails);
		}

        [HttpPost]
        public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBooking = await bookingService.CreateBookingAsync(request);

            return CreatedAtAction(nameof(GetBookingAsync), new { id = createdBooking.Id }, createdBooking);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateBookingAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var booking = await bookingService.UpdateBookingAsync(request);
            //return Ok(booking);

            return StatusCode(501);
		}

		[Authorize]
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

		[Authorize]
		[HttpPatch("{id}/check-in")]
		public async Task<IActionResult> CheckInAsync(int id)
		{
            var booking = await bookingService.GetByIdAsync(id);

            if(booking is null)
            {
                return NotFound();
            }

            await bookingService.CheckInAsync(id);

			return NoContent();
		}

		[Authorize]
		[HttpPatch("{id}/check-out")]
		public async Task<IActionResult> CheckOutAsync(int id)
		{
			var booking = await bookingService.GetByIdAsync(id);

			if (booking is null)
			{
				return NotFound();
			}

            await bookingService.CheckOutAsync(id);

			return NoContent();
		}
	}
}
