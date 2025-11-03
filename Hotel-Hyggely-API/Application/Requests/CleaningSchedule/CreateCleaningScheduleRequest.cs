namespace Application.Requests.CleaningSchedule
{
    public class CreateCleaningScheduleRequest
    {
        public int RoomId { get; set; }
        public bool Cleaned { get; set; }
        public DateTime CleaningDate { get; set; }
    }
}
