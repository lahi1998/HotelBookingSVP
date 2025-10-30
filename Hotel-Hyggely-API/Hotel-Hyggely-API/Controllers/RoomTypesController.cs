using Application.Dtos.Booking;
using Application.Dtos.Customer;
using Application.Dtos.RoomType;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly RoomTypeService roomTypeService;

        public RoomTypesController(RoomTypeService roomTypeService)
        {
            this.roomTypeService = roomTypeService;
        }

		[HttpGet]
		public async Task<IActionResult> GetAllRoomTypesAsync()
		{
			var roomTypes = await roomTypeService.GetAllAsync();

			var RoomTypeDto = roomTypes.Select(r => new RoomTypeDto
			{
				ID = r.ID,
				Name = r.Name,
				Price = r.Price
			});

			return Ok(RoomTypeDto);
		}
	}
}
