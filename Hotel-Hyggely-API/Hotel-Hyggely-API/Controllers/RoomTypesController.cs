using Application.Dtos.RoomType;
using Application.Requests.RoomType;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
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
		public async Task<IActionResult> GetAllAsync()
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

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			return StatusCode(501);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] CreateRoomTypeRequest request)
		{
			return StatusCode(501);
		}

		[Authorize]
		[HttpPut]
		public async Task<IActionResult> PutAsync([FromBody] UpdateRoomTypeRequest request)
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
