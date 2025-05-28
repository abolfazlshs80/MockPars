using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockPars.Application.DTO.RecordData;
using MockPars.Application.DTO.Users;
using MockPars.Application.Services.Interfaces;

namespace MockPars.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecordDataController(IRecordDataService RecordDataService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateRecordDataDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await RecordDataService.CreateRecordData(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRecordDataDto model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await RecordDataService.UpdateRecordData(model, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
    

            var result = await RecordDataService.DeleteRecordData(id, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("{id}/{columnId}")]
        public async Task<IActionResult> Get(int id,int columnId, CancellationToken ct)
        {


            var result = await RecordDataService.GetRecordDataById(id, columnId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }

        [HttpGet("{columnId}")]
        public async Task<IActionResult> GetAll(int columnId, CancellationToken ct)
        {


            var result = await RecordDataService.GetRecordDataByColumnId(columnId, ct);
            if (result.IsError)
                return BadRequest(string.Join(",", result.Errors.Select(a => a.Description)));

            return Ok(result.Value);
        }
    }
}
