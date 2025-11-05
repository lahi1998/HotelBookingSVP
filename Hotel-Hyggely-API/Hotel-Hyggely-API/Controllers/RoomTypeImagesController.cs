using Application.Dtos.RoomType;
using Application.Requests.RoomTypeImage;
using Application.Services;
using Hotel_Hyggely_API.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeImagesController : ControllerBase
    {
        private readonly RoomTypeImageService imageService;

        public RoomTypeImagesController(RoomTypeImageService imageService)
        {
            this.imageService = imageService;
        }

		[HttpGet("/api/roomtypes/{roomTypeId}/images")]
		public async Task<IActionResult> GetImagesForRoomTypeAsync(int roomTypeId)
		{
			var roomTypeImages = await imageService.GetByRoomTypeIdAsync(roomTypeId);

			return Ok(roomTypeImages);
		}

		[Authorize]
		[HttpPost("/api/roomtypes/images")]
		public async Task<IActionResult> PostImageAsync([FromForm] RoomTypeImageUploadRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var results = new List<RoomTypeImageDto>();

			foreach (var image in request.Images)
			{
				using var ms = new MemoryStream();
				await image.CopyToAsync(ms);

				var createRequest = new CreateImageRequest
				{
					RoomTypeId = request.RoomTypeId,
					ImgData = ms.ToArray(),
					FileType = Path.GetExtension(image.FileName).TrimStart('.')
				};

				var createdRoomTypeImage = await imageService.CreateAsync(createRequest);
				results.Add(createdRoomTypeImage);
			}

			return Ok(results);
		}

		[Authorize]
		[HttpDelete("/api/roomtypes/images/{id}")]
		public async Task<IActionResult> DeleteImageAsync(int id)
		{
			var result = await imageService.DeleteAsync(id);

			if (!result)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}
