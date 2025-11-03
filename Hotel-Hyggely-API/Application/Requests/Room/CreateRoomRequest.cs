namespace Application.Requests.Room
{
    public class CreateRoomRequest
    {
        public required string RoomTypeName { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public int BedAmount { get; set; }
    }
}
