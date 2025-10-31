using Application.Dtos.Room;
using Application.Dtos.RoomType;
using Application.Services;
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
	}
}