using ErrorOr;
using MockPars.Application.DTO.Table;

namespace MockPars.Application.Services.Interfaces;

public interface ITableService
{

    Task<ErrorOr<int>> CreateTable(CreateTableDto model, CancellationToken ct);
    Task<ErrorOr<int>> UpdateTable(UpdateTableDto model, CancellationToken ct);
    Task<ErrorOr<bool>> DeleteTable(int id, CancellationToken ct);
    Task<ErrorOr<TableItemDto>> GetTableById(int id, int databaseId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<TableItemDto>>> GetTablesByDatabaseId(int databaseId, CancellationToken ct);
}