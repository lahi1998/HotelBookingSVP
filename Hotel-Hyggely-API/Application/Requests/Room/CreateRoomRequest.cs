using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Room
{
    public class CreateRoomRequest : IValidatableObject
	{
        [Required]
        public required string RoomTypeName { get; set; }
		[Required]
		public int Number { get; set; }
		[Required]
		public int Floor { get; set; }
		[Required]
		public int BedAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
			if (Number <= 0)
			{
				yield return new ValidationResult(
					"Room Number cannot be 0 or lower",
					new[] { nameof(Number) });
			}
			if (Number <= 0)
			{
				yield return new ValidationResult(
					"Floor Number cannot be 0 or lower",
					new[] { nameof(Floor) });
			}
			if (Number <= 0)
			{
				yield return new ValidationResult(
					"BedAmount cannot be 0 or lower",
					new[] { nameof(BedAmount) });
			}
		}
    }
}
