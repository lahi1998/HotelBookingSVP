using System.ComponentModel.DataAnnotations;

namespace Application.Requests.RoomType
{
    public class UpdateRoomTypeRequest : IValidatableObject
	{
		[Required]
        public int Id { get; set; }
		[Required]
		public required string Name { get; set; }
		[Required]
		public decimal Price { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Price <= 0)
			{
				yield return new ValidationResult(
					"Price cannot be 0 or lower",
					new[] { nameof(Price) });
			}
		}
	}
}
