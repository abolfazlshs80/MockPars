using System.Linq.Expressions;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {


        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
 

    public class DatabasesRepository : Repository<Databases>, IDatabasesRepository
    {


        public DatabasesRepository(AppDbContext context) : base(context)
        {
        }
    }

    public class ColumnsRepository : Repository<Columns>, IColumnsRepository
    {


        public ColumnsRepository(AppDbContext context) : base(context)
        {
        }
    }
    public class TablesRepository : Repository<Tables>, ITablesRepository
    {


        public TablesRepository(AppDbContext context) : base(context)
        {
        }
    }
}
