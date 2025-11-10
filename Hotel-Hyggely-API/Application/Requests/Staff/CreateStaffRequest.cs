using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Staff
{
    public class CreateStaffRequest
    {
        [Required]
        public required string Role { get; set; }
		[Required]
		public required string UserName { get; set; }
		[Required, MinLength(6, ErrorMessage = "Password should contatin at least 6 characters")]
		public required string Password { get; set; }
		[Required]
		public required string FullName { get; set; }
    }
}
