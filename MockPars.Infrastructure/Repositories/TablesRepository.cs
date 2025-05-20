using Microsoft.EntityFrameworkCore;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories;

public class TablesRepository : Repository<Tables>, ITablesRepository
{

    private readonly AppDbContext _context;
    public TablesRepository(AppDbContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<Tables> GetByIdAsync(int id, int databaseId, CancellationToken ct)
    {
     return  await _context.Tables.Where(_ => _.Id.Equals(id) && _.DatabasesId.Equals(databaseId)).FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<Tables>> GetByDatabaseIdAsync(int databaseId, CancellationToken ct)
    {
        return await _context.Tables.Where(_ =>  _.DatabasesId.Equals(databaseId)).ToListAsync(ct);

    }
}