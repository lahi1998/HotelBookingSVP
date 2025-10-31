namespace Application.Dtos.Room
{
    public class AvailableRoomDto
    {
        public int ID { get; set; }
        public int RoomTypeId { get; set; } // TODO: Returner dette fra available rooms endpoint
    }
}
