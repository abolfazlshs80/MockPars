using MockPars.Domain.Models;

namespace MockPars.Domain.Interface;

public interface IRecordDataRepository : IRepository<RecordData>
{
    Task<RecordData> GetByIdAsync(int id, int columnId, CancellationToken ct);
    Task<IEnumerable<RecordData>> GetByColumnIdAsync(int columnId, CancellationToken ct);
}