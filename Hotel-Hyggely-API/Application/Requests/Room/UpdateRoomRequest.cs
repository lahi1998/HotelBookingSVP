using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Room
{
    public class UpdateRoomRequest
    {
        [Required]
        public int Id { get; set; }
		[Required]
		public required string RoomTypeName { get; set; }
		[Required]
		public int Number { get; set; }
		[Required]
		public int Floor { get; set; }
		[Required]
		public int BedAmount { get; set; }
		[Required]
		public DateTime LastCleanedDate { get; set; }
    }
}
