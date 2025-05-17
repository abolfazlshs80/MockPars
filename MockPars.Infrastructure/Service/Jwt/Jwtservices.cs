using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Models.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static SampleWebApi.Repository.Models.comment.Jwtservices;

namespace SampleWebApi.Repository.Models.comment
{
    public class Jwtservices( IOptions<ConfigJwtDto> jwtConfigureOptions) : IJwtService
    {
        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigureOptions.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
        
            };

            var token = new JwtSecurityToken(
                issuer: jwtConfigureOptions.Value.Issuer,
                audience: jwtConfigureOptions.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);
            var symmetricSecurityKey = securityKey;
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtConfigureOptions.Value.Issuer,
                audience: jwtConfigureOptions.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


        }

        public interface IJwtService
        {
            public string GenerateJwtToken(User user);
        }
    }
}
