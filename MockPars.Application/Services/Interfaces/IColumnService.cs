using ErrorOr;
using MockPars.Application.DTO.Column;

namespace MockPars.Application.Services.Interfaces;

public interface IColumnService
{
    Task<List<Dictionary<string, string>>> GetAllRowDataAsync(int tableId, CancellationToken ct);
    Task<ErrorOr<int>> CreateColumn(CreateColumnDto model, CancellationToken ct);
    Task<ErrorOr<int>> UpdateColumn(UpdateColumnDto model, CancellationToken ct);
    Task<ErrorOr<bool>> DeleteColumn(int id, CancellationToken ct);
    Task<ErrorOr<ColumnItemDto>> GetColumnById(int id, int tableId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<ColumnItemDto>>> GetColumnsByTableId(int tableId, CancellationToken ct);
    Task<ErrorOr<bool>> InsertDataByTableId(int tableId, List<ColumnValues> columns ,CancellationToken ct);
}