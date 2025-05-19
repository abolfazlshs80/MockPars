using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories;

public class ColumnsRepository : Repository<Columns>, IColumnsRepository
{


    public ColumnsRepository(AppDbContext context) : base(context)
    {
    }
}