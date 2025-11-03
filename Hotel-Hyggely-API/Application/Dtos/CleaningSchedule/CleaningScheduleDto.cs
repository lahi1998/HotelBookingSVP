namespace Application.Dtos.CleaningSchedule
{
    public class CleaningScheduleDto
    {
		public int Id { get; set; }
		public bool Cleaned { get; set; }
		public DateTime CleaningDate { get; set; }
        public int RoomNumber { get; set; }
        public int RoomFloor { get; set; }
    }
}
