namespace Application.Requests.RoomType
{
    public class UpdateRoomTypeRequest
    {
        public int Id { get; set; }
		public required string Name { get; set; }
		public decimal Price { get; set; }
	}
}
