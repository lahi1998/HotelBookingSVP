using Application.Dtos.RoomType;
using Domain.Entities;

namespace Application.Dtos.Room
{
    public class RoomDetailsDto
    {
		public int ID { get; set; }
		public required RoomTypeDto RoomType { get; set; }
		public DateTime LastCleanedDate { get; set; }
		public int Number { get; set; }
		public int Floor { get; set; }
		public int BedAmount { get; set; }
	}
}
