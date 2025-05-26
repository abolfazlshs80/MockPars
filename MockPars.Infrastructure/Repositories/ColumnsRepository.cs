using Microsoft.EntityFrameworkCore;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories;

public class ColumnsRepository : Repository<Columns>, IColumnsRepository
{

    private readonly AppDbContext _context;
    public ColumnsRepository(AppDbContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<Columns> GetByIdAsync(int id, int tableId, CancellationToken ct)
    {
        return await _context.Columns.Where(_ => _.Id.Equals(id) && _.TablesId.Equals(tableId)).FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<Columns>> GetByTableIdAsync(int tableId, CancellationToken ct)
    {
        return await _context.Columns.Where(_ => _.TablesId.Equals(tableId)).ToListAsync(ct);
    }

    public async Task<List<Columns>> GetAllRowDataAsync(int tableId, CancellationToken ct)
    {
       return  await _context.Columns
            .Where(c => c.TablesId == tableId)
            .Include(c => c.RecordData)
            .ToListAsync(ct);
    }
}