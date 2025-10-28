namespace Domain.Entities
{
    public class CleaningSchedule
    {
        public int ID { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public DateTime CleaningDate { get; set; }
        public bool Cleaned { get; set; }
    }
}
