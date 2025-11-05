using System.ComponentModel.DataAnnotations;

namespace Application.Requests.RoomType
{
    public class UpdateRoomTypeRequest
    {
		[Required]
        public int Id { get; set; }
		[Required]
		public required string Name { get; set; }
		[Required]
		public decimal Price { get; set; }
	}
}
