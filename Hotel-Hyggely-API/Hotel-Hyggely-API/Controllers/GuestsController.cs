using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hotel_Hyggely_API.Controllers
{
	[Route("api/[controller]")]
	[Authorize(Roles = "Admin, Receptionist")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly GuestService guestService;

        public GuestsController(GuestService guestService)
        {
            this.guestService = guestService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var guests = await guestService.GetAllAsync();

            return Ok(guests);
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await guestService.DeleteAsync(id);

            return NoContent();
		}
    }
}
