using MockPars.Domain.Models;

namespace MockPars.Domain.Interface;

public interface IColumnsRepository : IRepository<Columns>
{
    Task<Columns> GetByIdAsync(int id, int tableId,CancellationToken ct);
    Task<IEnumerable<Columns>> GetByTableIdAsync( int tableId,CancellationToken ct);
}