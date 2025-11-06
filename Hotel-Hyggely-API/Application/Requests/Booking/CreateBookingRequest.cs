using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Booking
{
	public class CreateBookingRequest : IValidatableObject
	{
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
		[Required, Range(1, 50)]
		public int PersonCount { get; set; }
		public string? Comment { get; set; }
		[Required]
		public required string FullName { get; set; }
		[Required, EmailAddress]
		public required string Email { get; set; }
		[Required, Phone]
		public required string PhoneNumber { get; set; }
		[Required]
		public required IEnumerable<int> RoomIds { get; set; } = new List<int>();

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (StartDate < DateTime.Today)
			{
				yield return new ValidationResult(
					"StartDate cannot be older than current date",
					new[] { nameof(StartDate) });
			}
			if (EndDate <= StartDate)
			{
				yield return new ValidationResult(
					"EndDate must be later than StartDate.",
					new[] { nameof(EndDate), nameof(StartDate) });
			}
			if (!RoomIds.Any())
			{
				yield return new ValidationResult(
					"RoomIds needs at least 1 room",
					new[] { nameof(RoomIds) });
			}
		}
	}
}
