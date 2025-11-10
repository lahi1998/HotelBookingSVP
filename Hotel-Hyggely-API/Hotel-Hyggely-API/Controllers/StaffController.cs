using Application.Exeptions;
using Application.Requests.Staff;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Hyggely_API.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly StaffService staffService;

        public StaffController(StaffService staffService)
        {
            this.staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var staff = await staffService.GetAllAsync();

            return Ok(staff);
		}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
			var staff = await staffService.GetByIdAsync(id);

            if(staff is null)
            {
                return NotFound();
			}

            return Ok(staff);
		}

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateStaffRequest request)
        {
            try
            {
                var staff = await staffService.CreateAsync(request);

                return CreatedAtAction(nameof(GetAsync), new { id = staff.Id }, staff);
            }
            catch (UserNameTakenExeption ex)
            {
                return Conflict(new { ex.Message });
            }
		}

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateStaffRequest request)
        {
			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
			}

            var staff = await staffService.UpdateAsync(request);

			if (staff is null)
			{
				return NotFound();
			}

			return Ok(staff);
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
			var existingStaff = await staffService.GetByIdAsync(id);

            if (existingStaff is null)
            {
                return NotFound();
            }

            await staffService.DeleteAsync(id);

            return NoContent();
		}
    }
}
