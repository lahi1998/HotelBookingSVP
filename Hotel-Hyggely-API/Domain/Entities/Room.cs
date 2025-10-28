namespace Domain.Entities
{
    public class Room
    {
        public int ID { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; }

        public DateTime LastCleanedDate { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public int BedAmount { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<CleaningSchedule> CleaningSchedules { get; set; } = new List<CleaningSchedule>();
        public ICollection<RoomStatus> RoomStatuses { get; set; } = new List<RoomStatus>();
    }
}
