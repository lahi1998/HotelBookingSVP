using System.ComponentModel.DataAnnotations;

namespace Application.Requests.CleaningSchedule
{
    public class CreateCleaningScheduleRequest : IValidatableObject
	{
        [Required]
        public int RoomId { get; set; }
		[Required]
		public DateTime CleaningDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
			if (CleaningDate < DateTime.Today)
			{
				yield return new ValidationResult(
					"CleaningDate cannot be older than current date",
					new[] { nameof(CleaningDate) });
			}
		}
    }
}
