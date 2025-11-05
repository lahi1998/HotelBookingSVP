using Application.Dtos;
using Application.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await authService.AuthenticateAsync(request);

            if (token == null)
            {
                return Unauthorized("Ugyldigt brugernavn eller kodeord.");
            }

            return Ok(new AuthResponeDto { Token = token });
        }
    }
}
