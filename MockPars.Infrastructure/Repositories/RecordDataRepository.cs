using Microsoft.EntityFrameworkCore;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories;

public class RecordDataRepository : Repository<RecordData>, IRecordDataRepository
{

    private readonly AppDbContext _context;
    public RecordDataRepository(AppDbContext context) : base(context)
    {
        this._context = context;
    }


    public async Task<RecordData> GetByIdAsync(int id, int columnId, CancellationToken ct)
    {
        return await  _context.RecordData.Where(a => a.ColumnsId.Equals(columnId) && a.Id.Equals(id)).FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<RecordData>> GetByColumnIdAsync(int columnId, CancellationToken ct)
    {
        return await _context.RecordData.Where(a => a.ColumnsId.Equals(columnId) ).ToListAsync(ct);
    }
}