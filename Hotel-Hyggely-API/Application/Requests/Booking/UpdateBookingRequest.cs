using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Booking
{
    public class UpdateBookingRequest : IValidatableObject
    {
        [Required]
        public int Id { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
		[Required, Range(1, 100000)]
		public decimal TotalPrice { get; set; }
		[Required, Range(1, 10)]
		public int PersonCount { get; set; }
        public string? Comment { get; set; }
		[Required]
		public required string FullName { get; set; }
		[Required, EmailAddress]
		public required string Email { get; set; }
		[Required, Phone]
		public required string PhoneNumber { get; set; }
		[Required]
		public IEnumerable<int> RoomIds { get; set; } = [];

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
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
