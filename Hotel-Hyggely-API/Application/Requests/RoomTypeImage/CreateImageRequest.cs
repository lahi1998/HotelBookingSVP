using System.ComponentModel.DataAnnotations;

namespace Application.Requests.RoomTypeImage
{
    public class CreateImageRequest
	{
        [Required]
        public int RoomTypeId { get; set; }
		[Required]
		public required byte[] ImgData { get; set; }
        [Required]
        public required string FileType { get; set; }
	}
}
