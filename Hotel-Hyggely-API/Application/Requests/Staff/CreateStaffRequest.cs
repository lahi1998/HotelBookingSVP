using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Staff
{
    public class CreateStaffRequest
    {
        [Required]
        public required string Role { get; set; }
		[Required]
		public required string UserName { get; set; }
		[Required]
		public required string Password { get; set; }
		[Required]
		public required string FullName { get; set; }
    }
}
