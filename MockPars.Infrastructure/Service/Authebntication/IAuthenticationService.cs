using ErrorOr;
using MockPars.Application.Generator;
using MockPars.Application.Static.Message;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Context;
using MockPars.Infrastructure.Models.Authebntication;
using MockPars.Infrastructure.Service.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPars.Infrastructure.Service.Authebntication
{


    public class AuthenticationService(IUnitOfWork unitOfWork, IJwtService jwtService) : IAuthenticationService
    {
        public async Task<ErrorOr<string>> AuthenticateAsync(UserLoginDto model, CancellationToken ct)
        {
            var user = await unitOfWork.UserRepository.GetAsync(model.UserName, model.Password, ct);
            if (user == null)
                return Error.NotFound("", description: UserMessageStatic.NotFound_User);
            var token = jwtService.GenerateJwtToken(user);
            return token;

        }

        public async Task<ErrorOr<string>> RegisterAsync(UserRegisterDto model, CancellationToken ct)
        {
            var user = await unitOfWork.UserRepository.GetAsync(model.UserName, ct);
            if (user != null)
                return Error.NotFound("", description: UserMessageStatic.Found_User);
            var newUser = new User()
            {
                Id = NameGenerator.GenerateUniqueCode(),
                Password = model.Password,
                UserName = model.UserName,
            };
            await unitOfWork.UserRepository.CreateAsync(newUser, ct);
            await unitOfWork.SaveChangesAsync(ct);

            var token = jwtService.GenerateJwtToken(newUser);

            return token;
        }
    }

    public interface IAuthenticationService
    {
        Task<ErrorOr<string>> AuthenticateAsync(UserLoginDto model, CancellationToken ct);
        Task<ErrorOr<string>> RegisterAsync(UserRegisterDto model, CancellationToken ct);
    }
}
