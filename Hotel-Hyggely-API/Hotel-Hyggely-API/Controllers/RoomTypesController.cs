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
        private readonly RoomTypeImageService imageService;

        public RoomTypesController(RoomTypeService roomTypeService, RoomTypeImageService imageService)
        {
            this.roomTypeService = roomTypeService;
            this.imageService = imageService;
        }

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var roomTypes = await roomTypeService.GetAllAsync();

			return Ok(roomTypes);
		}

		[HttpGet("images")]
		public async Task<IActionResult> GetAllImagesAsync()
		{
			var roomTypeImages = await imageService.GetAllAsync();

			return Ok(roomTypeImages);
		}

		[HttpGet("{id}/images")]
		public async Task<IActionResult> GetImagesAsync(int id)
		{
			var roomTypeImages = await imageService.GetByRoomTypeIdAsync(id);

			return Ok(roomTypeImages);
		}

		[Authorize]
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

		[Authorize]
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

		[Authorize]
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

		[Authorize]
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
