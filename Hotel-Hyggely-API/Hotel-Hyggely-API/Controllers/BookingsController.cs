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

            var BookingDetails = bookings.Select(b => new BookingListItemDto
            {
                Id = b.ID,
                RoomCount = b.Rooms.Count,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Customer = new CustomerDto
                {
                    FullName = b.Customer!.FullName,
                    Email = b.Customer.Email,
                    PhoneNumber = b.Customer.PhoneNumber
                }
            });

			return Ok(BookingDetails);
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetBookingAsync(int id)
		{
			var b = await bookingService.GetByIdWithDetailsAsync(id);

            if(b is null)
            {
                return NotFound();
            }

			var BookingDetails = new BookingDetailsDto
			{
				Id = b.ID,
				StartDate = b.StartDate,
				EndDate = b.EndDate,
                Status = b.CheckInStatus,
                Price = b.TotalPrice,
                PersonCount = b.PersonCount,
                Comment = b.Comment,
				Customer = new CustomerDto
				{
					FullName = b.Customer!.FullName,
					Email = b.Customer.Email,
					PhoneNumber = b.Customer.PhoneNumber
				},
                Rooms = b.Rooms.Select(r => new RoomDetailsDto
                {
                    ID = r.ID,
                    Number = r.Number,
                    BedAmount = r.BedAmount,
                    Floor = r.Floor,
                    LastCleanedDate = r.LastCleanedDate,
                    RoomType = new RoomTypeDto
                    {
                        ID = r.RoomType!.ID,
                        Name = r.RoomType.Name,
                        Price = r.RoomType.Price
					}
				})

			};

			return Ok(BookingDetails);
		}

		[HttpPost]
        public async Task<IActionResult> CreateBookingAsync([FromBody]CreateBookingRequest request)
        {
            var booking = await bookingService.CreateBookingAsync(request);

            var bookingDto = new BookingDto
            {
                Id = booking.ID,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Price = booking.TotalPrice,
                PersonCount = booking.PersonCount,
                Comment = booking.Comment,
                Customer = new CustomerDto
                {
                    FullName = booking.Customer!.FullName,
                    Email = booking.Customer.Email,
                    PhoneNumber = booking.Customer.PhoneNumber
                },
                RoomIds = booking.Rooms.Select(r => r.ID).ToList()
            };

			return Ok(bookingDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookingAsync()
        {
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
