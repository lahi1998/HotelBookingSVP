using System.ComponentModel.DataAnnotations;

namespace Hotel_Hyggely_API.Requests
{
    public class RoomTypeImageUploadRequest
    {
		[Required]
		public int RoomTypeId { get; set; }

		[Required]
		public required IEnumerable<IFormFile> Images { get; set; }
	}
}
