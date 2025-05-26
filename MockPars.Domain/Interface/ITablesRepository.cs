using MockPars.Domain.Models;

namespace MockPars.Domain.Interface;

public interface ITablesRepository : IRepository<Tables>
{
    Task<Tables> GetByIdAsync(int id,int databaseId, CancellationToken ct);
    Task<IEnumerable<Tables>> GetByDatabaseIdAsync(int databaseId, CancellationToken ct);
    Task<Tables> GetColumnsByIdAsync(int tableId, CancellationToken ct);
}