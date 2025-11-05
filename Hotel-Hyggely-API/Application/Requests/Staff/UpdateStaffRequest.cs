using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Staff
{
    public class UpdateStaffRequest
    {
		[Required]
		public int Id { get; set; }
		[Required]
		public required string Role { get; set; }
		[Required]
		public required string UserName { get; set; }
		public string? Password { get; set; }
		[Required]
		public required string FullName { get; set; }
	}
}
