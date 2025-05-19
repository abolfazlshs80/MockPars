using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories;

public class DatabasesRepository : Repository<Databases>, IDatabasesRepository
{


    public DatabasesRepository(AppDbContext context) : base(context)
    {
    }
}