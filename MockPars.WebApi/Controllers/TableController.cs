using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockPars.Application.DTO.Table;
using MockPars.Application.DTO.Users;
using MockPars.Application.Services.Interfaces;

namespace MockPars.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TableController(ITableService TableService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateTableDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await TableService.CreateTable(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTableDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await TableService.UpdateTable(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
    

            var result = await TableService.DeleteTable(id, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("{id}/{databaseId}")]
        public async Task<IActionResult> Get(int id,int databaseId, CancellationToken ct)
        {


            var result = await TableService.GetTableById(id, databaseId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("{databaseId}")]
        public async Task<IActionResult> GetAll(int databaseId, CancellationToken ct)
        {


            var result = await TableService.GetTablesByDatabaseId(databaseId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
    }
}
