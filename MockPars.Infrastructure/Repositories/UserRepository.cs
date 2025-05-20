using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> Create(User user, CancellationToken ct)
        {
           await _context.User.AddAsync(user, ct);
           return true;
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken ct)
        {
         return   await _context.User.AnyAsync(a => a.Id.Equals(id), ct);
        }
    }
}
