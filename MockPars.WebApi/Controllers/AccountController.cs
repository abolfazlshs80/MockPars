using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockPars.Application.DTO.Column;
using MockPars.Application.DTO.Users;
using MockPars.Application.Extention;
using MockPars.Application.Services.Interfaces;
using MockPars.Infrastructure.Models.Authebntication;
using MockPars.Infrastructure.Service.Authebntication;

namespace MockPars.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IAuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto userLoginDto, CancellationToken ct)
        {

            if (!ModelState.IsValid)
                return BadRequest(userLoginDto);
            var result = await authenticationService.AuthenticateAsync(userLoginDto, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));
            return Ok(result.Value);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userLoginDto, CancellationToken ct)
        {

            if (!ModelState.IsValid)
                return BadRequest(userLoginDto);
            var result = await authenticationService.RegisterAsync(userLoginDto, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));
            return Ok(result.Value);
        }

        [HttpGet("test")]
        [Authorize]
        public async Task<IActionResult> test()
        {
            var u = User.GetUserId();

            return Ok();
        }
    }
}
