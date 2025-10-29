using System.ComponentModel.DataAnnotations;

namespace Application.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Brugernavn er påkrævet")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Kodeord er påkrævet")]
        public required string Password { get; set; }
    }
}
