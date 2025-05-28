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

        public async Task<bool> CreateAsync(User user, CancellationToken ct)
        {
            await _context.User.AddAsync(user, ct);
            return true;
        }

        public async Task<User> GetAsync(string username, string password, CancellationToken ct)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password, ct);
        }

        public async Task<User> GetAsync(string username, CancellationToken ct)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.UserName == username , ct);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken ct)
        {
            return await _context.User.AnyAsync(a => a.Id.Equals(id), ct);
        }
    }
}
