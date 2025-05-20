using ErrorOr;
using MockPars.Application.DTO.Database;
using MockPars.Application.DTO.Users;

namespace MockPars.Application.Services.Interfaces;

public interface IDatabaseService

{

    Task<ErrorOr<int>> CreateDatabase(CreateDatabaseDto model, CancellationToken ct);
    Task<ErrorOr<int>> UpdateDatabase(UpdateDatabaseDto model, CancellationToken ct);
    Task<ErrorOr<bool>> DeleteDatabase(int id, CancellationToken ct);
    Task<ErrorOr<DatabaseItemDto>> GetDatabaseById(int id, string userId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<DatabaseItemDto>>> GetDatabasesByUserId(string userId, CancellationToken ct);
}