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
			var rooms = await roomService.GetByPeriod(fromDate, toDate);

			var RoomDto = rooms.Select(r => new RoomDetailsDto
			{
				ID = r.ID,
				BedAmount = r.BedAmount,
				Floor = r.Floor,
				LastCleanedDate = r.LastCleanedDate,
				Number = r.Number,
				RoomType = new RoomTypeDto
				{
					ID = r.RoomType!.ID,
					Name = r.RoomType.Name,
					Price = r.RoomType.Price
				}
			});

			return Ok(RoomDto);
		}
	}
}