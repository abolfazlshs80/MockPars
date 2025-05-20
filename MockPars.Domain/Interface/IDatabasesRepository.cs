using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockPars.Domain.Models;

namespace MockPars.Domain.Interface
{
    public interface IDatabasesRepository :IRepository<Databases>
    {
        Task<IEnumerable<Databases>> GetByUserIdAsync(string userId, CancellationToken ct);
        Task<Databases> GetByIdAsync(int id, string userId, CancellationToken ct);
    }
}
