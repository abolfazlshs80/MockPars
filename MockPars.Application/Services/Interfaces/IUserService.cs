using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MockPars.Application.DTO.Users;

namespace MockPars.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<ErrorOr<string>> Register(RegisterDto model, CancellationToken ct);
    }
}
