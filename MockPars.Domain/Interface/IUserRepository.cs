
using MockPars.Domain.Models;

namespace MockPars.Domain.Interface;

public interface IUserRepository : IRepository<User>
{
    Task<bool> Create(User user, CancellationToken ct);
    Task<bool> ExistsAsync(string id, CancellationToken ct);
}