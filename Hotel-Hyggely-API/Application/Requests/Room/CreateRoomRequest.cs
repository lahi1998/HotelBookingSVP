using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Room
{
    public class CreateRoomRequest
    {
        [Required]
        public required string RoomTypeName { get; set; }
		[Required]
		public int Number { get; set; }
		[Required]
		public int Floor { get; set; }
		[Required]
		public int BedAmount { get; set; }
    }
}
