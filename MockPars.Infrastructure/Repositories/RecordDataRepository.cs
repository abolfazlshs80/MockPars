using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;
using System.Text.RegularExpressions;

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
        return await _context.RecordData.Where(a => a.ColumnsId.Equals(columnId) && a.Id.Equals(id)).FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<RecordData>> GetByColumnIdAsync(int columnId, CancellationToken ct)
    {
        return await _context.RecordData.Where(a => a.ColumnsId.Equals(columnId)).ToListAsync(ct);
    }

    public async Task<int> GetLastRowByColumnIdAsync(int columnId, CancellationToken ct)
    {


        var res = await _context.RecordData.Where(a => a.ColumnsId.Equals(columnId)).OrderByDescending(_ => _.RowIndex).FirstOrDefaultAsync(ct);
        return res is null? 1 : res.RowIndex + 1;
    }
}

