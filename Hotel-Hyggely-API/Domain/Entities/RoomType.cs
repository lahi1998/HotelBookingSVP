namespace Domain.Entities
{
    public class RoomType
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
