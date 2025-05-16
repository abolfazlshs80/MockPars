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
}
