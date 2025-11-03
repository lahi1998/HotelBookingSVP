namespace Application.Requests.CleaningSchedule
{
    public class UpdateCleaningScheduleRequest
    {
        public int Id { get; set; }
		public int RoomId { get; set; }
		public bool Cleaned { get; set; }
		public DateTime CleaningDate { get; set; }
	}
}
