using Application.Requests.Staff;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
			return StatusCode(501);
		}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
			return StatusCode(501);
		}

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateStaffRequest request)
        {
			return StatusCode(501);
		}

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateStaffRequest request)
        {
			return StatusCode(501);
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
			return StatusCode(501);
		}
    }
}
