using Microsoft.EntityFrameworkCore;
using MockPars.Application.DTO.Database;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories;

public class DatabasesRepository : Repository<Databases>, IDatabasesRepository
{
    private readonly AppDbContext context;

    public DatabasesRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Databases> GetByIdAsync(int id, string userId, CancellationToken ct)
    {
        return await context.Databases.Where(a => a.Id.Equals(id) && a.UserId.Equals(userId))
            .Include(_=>_.Tables)
            .ThenInclude(_=>_.Columns)
            .ThenInclude(_=>_.RecordData)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(ct)??default;
    }
    public async Task<IEnumerable<Databases>> GetByUserIdAsync( string userId, CancellationToken ct)
    {
        return await context.Databases.Where(a => a.UserId.Equals(userId)).Select(_=> new Databases{Id = _.Id,DatabaseName = _.DatabaseName,Slug = _.Slug }).ToListAsync(ct);
    }
}