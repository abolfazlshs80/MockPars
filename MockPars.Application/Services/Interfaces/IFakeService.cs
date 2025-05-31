using ErrorOr;
using MockPars.Application.DTO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPars.Application.Services.Interfaces
{
    public interface IFakeService
    {
        Task<ErrorOr<int>> GenerateFakeData(int tableId,int count, CancellationToken ct);
        Task<ErrorOr<Dictionary<string,int>>> GetFakeTypes( CancellationToken ct);
        //Task
    }
}
