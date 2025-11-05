using Application.Requests.CleaningSchedule;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
	[Authorize(Roles = "Admin, Receptionist, Cleaning")]
	[Route("api/[controller]")]
    [ApiController]
    public class CleaningSchedulesController : ControllerBase
    {
        private readonly CleaningScheduleService cleaningScheduleService;

        public CleaningSchedulesController(CleaningScheduleService cleaningScheduleService)
        {
            this.cleaningScheduleService = cleaningScheduleService;
        }

		[Authorize(Roles = "Admin, Receptionist")]
		[HttpGet("/api/bookings/{bookingId}/cleaningschedules")]
		public async Task<IActionResult> GetForBooking(int bookingId)
		{
			var cleaningSchedules = await cleaningScheduleService.GetByBookingIdAsync(bookingId);

			return Ok(cleaningSchedules);
		}

		[HttpGet("pending")]
        public async Task<IActionResult> GetPendingAsync()
        {
			var cleaningSchedules = await cleaningScheduleService.GetPendingWithRoomAsync();

			return Ok(cleaningSchedules);
		}

		[Authorize(Roles = "Admin, Receptionist")]
		[HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
			var cleaningSchedule = await cleaningScheduleService.GetByIdAsync(id);

			if(cleaningSchedule is null)
			{
				return NotFound();
			}

			return Ok(cleaningSchedule);
		}

		[Authorize(Roles = "Admin, Receptionist")]
		[HttpPost]
        public async Task<IActionResult> PostForRoomAsync([FromBody] CreateCleaningScheduleRequest request)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var createdCleaningSchedule = await cleaningScheduleService.CreateForRoomAsync(request);

			return CreatedAtAction(nameof(GetAsync), new { id = createdCleaningSchedule.Id }, createdCleaningSchedule);
		}

		[HttpPatch("{id}/finish")]
		public async Task<IActionResult> Finish(int id)
		{
			var succes = await cleaningScheduleService.MarkAsCleaned(id);

			if(!succes)
			{
				return NotFound();
			}

			return NoContent();
		}

		[Authorize(Roles = "Admin, Receptionist")]
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
			var result = await cleaningScheduleService.DeleteAsync(id);

			if (!result)
			{
				return NotFound();
			}

			return NoContent();
		}
    }
}
