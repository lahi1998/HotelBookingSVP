using Application.Dtos.Room;
using Application.Dtos.RoomType;
using Application.Requests.Room;
using Application.Requests.Staff;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
	[Authorize(Roles = "Admin, Receptionist")]
	[Route("api/[controller]")]
	[ApiController]
	public class RoomsController : ControllerBase
	{
		private readonly RoomService roomService;
		public RoomsController(RoomService roomService)
		{
			this.roomService = roomService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var room = await roomService.GetAllAsync();

			return Ok(room);
		}

		[AllowAnonymous]
		[HttpGet("available")]
		public async Task<IActionResult> GetAvailableRoomsAsync([FromQuery]DateTime fromDate, DateTime toDate)
		{
			var availableRooms = await roomService.GetAvailableByPeriod(fromDate, toDate);

			return Ok(availableRooms);
		}

		[HttpGet("availabledetailed")]
		public async Task<IActionResult> GetAvailableRoomsWithDetailsAsync([FromQuery] DateTime fromDate, DateTime toDate)
		{
			var availableRooms = await roomService.GetAvailableByPeriodWithDetails(fromDate, toDate);

			return Ok(availableRooms);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var room = await roomService.GetByIdAsync(id);

			if(room is null)
			{
				return NotFound();
			}

			return Ok(room);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] CreateRoomRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var room = await roomService.CreateAsync(request);

			return CreatedAtAction(nameof(GetAsync), new { id = room.ID }, room);
		}

		[Authorize(Roles = "Admin")]
		[HttpPut]
		public async Task<IActionResult> PutAsync([FromBody] UpdateRoomRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var room = await roomService.UpdateAsync(request);

			if (room is null)
			{
				return NotFound();
			}

			return Ok(room);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var result = await roomService.DeleteAsync(id);

			if (!result)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}