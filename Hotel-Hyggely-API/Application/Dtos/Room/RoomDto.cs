using Application.Dtos.RoomType;

namespace Application.Dtos.Room
{
    public class RoomDto
    {
		public int ID { get; set; }
		public int RoomTypeId { get; set; }
		public DateTime LastCleanedDate { get; set; }
		public int Number { get; set; }
		public int Floor { get; set; }
		public int BedAmount { get; set; }
	}
}
