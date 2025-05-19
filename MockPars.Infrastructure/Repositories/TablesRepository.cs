using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories;

public class TablesRepository : Repository<Tables>, ITablesRepository
{


    public TablesRepository(AppDbContext context) : base(context)
    {
    }
}