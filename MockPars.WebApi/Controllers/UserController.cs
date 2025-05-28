using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockPars.Application.DTO.Users;
using MockPars.Application.Services.Interfaces;

namespace MockPars.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> GetAll(RegisterDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await userService.Register(model, ct);
            if (result.IsError)
                return BadRequest(model);

            return Ok(result.Value);
        }

    }
}
