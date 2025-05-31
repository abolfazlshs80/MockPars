using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockPars.Application.DTO.Column;
using MockPars.Application.DTO.Users;
using MockPars.Application.Services.Implementation;
using MockPars.Application.Services.Interfaces;

namespace MockPars.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FakeController(IFakeService fakeService) : ControllerBase
    {
        [HttpGet("{tableId}/{count}")]
        public async Task<IActionResult> GenerateFakeData(int tableId, int count)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(model);

            var result = await fakeService.GenerateFakeData(tableId, count, HttpContext.RequestAborted);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);



        }
        [AllowAnonymous]
        [HttpGet("GetFakeTypes")]

        public async Task<IActionResult> GetFakeTypes(CancellationToken ct)
        {

            return Ok(await fakeService.GetFakeTypes(ct));
        }
    }
}
