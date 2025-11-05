using System.ComponentModel.DataAnnotations;

namespace Application.Requests.RoomType
{
    public class CreateRoomTypeRequest
    {
        [Required]
        public required string Name { get; set; }
		[Required]
		public decimal Price { get; set; }
    }
}
