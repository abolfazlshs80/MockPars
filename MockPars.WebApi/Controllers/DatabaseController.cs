using Microsoft.AspNetCore.Mvc;
using MockPars.Application.DTO.Database;
using MockPars.Application.DTO.Users;
using MockPars.Application.Services.Interfaces;

namespace MockPars.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController(IDatabaseService databaseService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateDatabaseDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await databaseService.CreateDatabase(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDatabaseDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await databaseService.UpdateDatabase(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
    

            var result = await databaseService.DeleteDatabase(id, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("{id}/{userId}")]
        public async Task<IActionResult> Get(int id,string userId, CancellationToken ct)
        {


            var result = await databaseService.GetDatabaseById(id,userId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(string userId, CancellationToken ct)
        {


            var result = await databaseService.GetDatabasesByUserId( userId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
    }
}
