namespace Application.Requests.Room
{
    public class UpdateRoomRequest
    {
        public int Id { get; set; }
        public required string RoomTypeName { get; set; }
		public int Number { get; set; }
		public int Floor { get; set; }
		public int BedAmount { get; set; }
        public DateTime LastCleanedDate { get; set; }
    }
}
