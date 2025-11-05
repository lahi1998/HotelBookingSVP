using System.ComponentModel.DataAnnotations;

namespace Application.Requests.CleaningSchedule
{
    public class CreateCleaningScheduleRequest
    {
        [Required]
        public int RoomId { get; set; }
		[Required]
		public DateTime CleaningDate { get; set; }
    }
}
