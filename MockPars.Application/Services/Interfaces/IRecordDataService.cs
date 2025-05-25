using ErrorOr;
using MockPars.Application.DTO.RecordData;

namespace MockPars.Application.Services.Interfaces;

public interface IRecordDataService
{
    Task<ErrorOr<int>> CreateRecordData(CreateRecordDataDto model, CancellationToken ct);
    Task<ErrorOr<int>> UpdateRecordData(UpdateRecordDataDto model, CancellationToken ct);
    Task<ErrorOr<bool>> DeleteRecordData(int id, CancellationToken ct);
    Task<ErrorOr<RecordDataItemDto>> GetRecordDataById(int id, int ColumnId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<RecordDataItemDto>>> GetRecordDataByColumnId(int ColumnId, CancellationToken ct);
}