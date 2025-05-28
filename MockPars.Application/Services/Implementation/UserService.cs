using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MockPars.Application.DTO.Users;
using MockPars.Application.Generator;
using MockPars.Application.Services.Interfaces;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;


namespace MockPars.Application.Services.Implementation
{
    public class UserService(IUnitOfWork unitOfWork):IUserService
    {
        public async Task<ErrorOr<string>> Register(RegisterDto model, CancellationToken ct)
        {
            var user = new User()
            {
                Id = NameGenerator.GenerateUniqueCode(),
                UserName = model.UserName,
                Password = model.Password
            };
            await unitOfWork.UserRepository.CreateAsync(user, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return user.Id;
        }
    }
}
