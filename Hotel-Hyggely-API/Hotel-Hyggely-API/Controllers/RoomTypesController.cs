using Application.Dtos.RoomType;
using Application.Requests.RoomType;
using Application.Requests.RoomTypeImage;
using Application.Services;
using Hotel_Hyggely_API.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly RoomTypeService roomTypeService;

        public RoomTypesController(RoomTypeService roomTypeService)
        {
            this.roomTypeService = roomTypeService;
        }

		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var roomTypes = await roomTypeService.GetAllAsync();

			return Ok(roomTypes);
		}

		
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var roomType = await roomTypeService.GetByIdAsync(id);

			if (roomType == null)
			{
				return NotFound();
			}

			return Ok(roomType);
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] CreateRoomTypeRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var createdRoomType = await roomTypeService.CreateAsync(request);

			return CreatedAtAction(nameof(GetAsync), new { id = createdRoomType.ID }, createdRoomType);
		}

		[HttpPut]
		public async Task<IActionResult> PutAsync([FromBody] UpdateRoomTypeRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var updatedRoomType = await roomTypeService.UpdateAsync(request);

			if (updatedRoomType == null)
			{
				return NotFound();
			}

			return Ok(updatedRoomType);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var result = await roomTypeService.DeleteAsync(id);

			if (!result)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}
