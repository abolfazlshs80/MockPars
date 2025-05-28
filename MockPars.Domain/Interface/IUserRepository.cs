
using MockPars.Domain.Models;

namespace MockPars.Domain.Interface;

public interface IUserRepository : IRepository<User>
{
    Task<bool> CreateAsync(User user, CancellationToken ct);
    Task<User> GetAsync(string username,string password, CancellationToken ct);
    Task<User> GetAsync(string username, CancellationToken ct);
    Task<bool> ExistsAsync(string id, CancellationToken ct);
}