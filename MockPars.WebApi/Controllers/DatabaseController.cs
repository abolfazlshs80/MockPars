using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MockPars.Application.DTO.Database;
using MockPars.Application.DTO.Table;
using MockPars.Application.DTO.Users;
using MockPars.Application.Extention;
using MockPars.Application.Services.Interfaces;

namespace MockPars.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController(IDatabaseService databaseService,ISqlProvider sqlProvider) : ControllerBase
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {


            var result = await databaseService.GetDatabaseById(id,User.GetUserId(), ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll( CancellationToken ct)
        {


            var result = await databaseService.GetDatabasesByUserId( User.GetUserId(), ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }


        [HttpPost("ConnectToDatabase")]
        [AllowAnonymous]
        public async Task<IActionResult> ConnectToDatabase(ConnectionDatabaseDto model,CancellationToken ct)
        {
            return Ok(await sqlProvider.GetTablesAsync(model,ct));
        }


        [HttpPost("FakeDataToTable")]
        [AllowAnonymous]
        public async Task<IActionResult> ConnectToDatabase(FakeDataToTableDto model, CancellationToken ct)
        {
            var res = await sqlProvider.AddFakeDataAsync(model);
            return Ok();
        }

        [HttpPost("GetColumnsByTable")]
        [AllowAnonymous]
        public async Task<IActionResult> ConnectToDatabase(GetColumnByTableDto model, CancellationToken ct)
        {
            var res = await sqlProvider.GetTableColumnAsync(model);
            return Ok(res);
        }
    }
}
