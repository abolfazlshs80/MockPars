using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockPars.Application.DTO.Column;
using MockPars.Application.DTO.Table;
using MockPars.Application.DTO.Users;
using MockPars.Application.Services.Interfaces;

namespace MockPars.WebApi.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ColumnController(IColumnService ColumnService,ISqlProvider sqlProvider) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateColumnDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await ColumnService.CreateColumn(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateColumnDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await ColumnService.UpdateColumn(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
    

            var result = await ColumnService.DeleteColumn(id, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("{id}/{tableId}")]
        public async Task<IActionResult> Get(int id,int tableId, CancellationToken ct)
        {


            var result = await ColumnService.GetColumnById(id, tableId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
        [HttpGet("GetRecord/{tableId}")]
        public async Task<IActionResult> GetRecord( int tableId, CancellationToken ct)
        {


            var result = await ColumnService.GetAllRowDataAsync(tableId, ct);
        

            return Ok(result);
        }
        [HttpGet("{tableId}")]
        public async Task<IActionResult> GetAll(int tableId, CancellationToken ct)
        {


            var result = await ColumnService.GetColumnsByTableId(tableId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpPost("InsertValue/{tableId}")]
        public async Task<IActionResult> InsertValue(int tableId,List<ColumnValues> columns, CancellationToken ct)
        {


            var result = await ColumnService.InsertDataByTableId(tableId,columns, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpPost("GetColumnsByTable")]
        [AllowAnonymous]
        public async Task<IActionResult> ConnectToDatabase(GetColumnByTableDto model, CancellationToken ct)
        {
            var res = await sqlProvider.GetTableColumnAsync(model);
            return Ok(res.Value);
        }
    }
}
