using Application.Dtos.Room;
using Application.Dtos.RoomType;
using Application.Requests.Room;
using Application.Requests.Staff;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoomsController : ControllerBase
	{
		private readonly RoomService roomService;
		public RoomsController(RoomService roomService)
		{
			this.roomService = roomService;
		}

		[HttpGet("available")]
		public async Task<IActionResult> GetAvailableRoomsAsync([FromQuery]DateTime fromDate, DateTime toDate)
		{
			var availableRooms = await roomService.GetAvailableByPeriod(fromDate, toDate);

			return Ok(availableRooms);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			return StatusCode(501);
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			return StatusCode(501);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] CreateRoomRequest request)
		{
			return StatusCode(501);
		}

		[Authorize]
		[HttpPut]
		public async Task<IActionResult> PutAsync([FromBody] UpdateRoomRequest request)
		{
			return StatusCode(501);
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			return StatusCode(501);
		}
	}
}