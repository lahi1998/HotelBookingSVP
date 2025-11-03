namespace Application.Requests.RoomType
{
    public class CreateRoomTypeRequest
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}
