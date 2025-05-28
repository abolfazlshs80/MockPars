using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MockPars.Domain.Models;
using MockPars.Infrastructure.Models.Jwt;

namespace MockPars.Infrastructure.Service.Jwt
{
    public class JwtService( IOptions<ConfigJwtDto> jwtConfigureOptions) : IJwtService
    {
        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigureOptions.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
        
            };
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

        public async Task< ClaimsPrincipal?> ValidateAndExtractClaimsAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtConfigureOptions.Value.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = jwtConfigureOptions.Value.Issuer,

                ValidateAudience = false,
                ValidAudience = jwtConfigureOptions.Value.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // برای دقت در اعتبار سنجی تاریخ انقضا
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                // توکن نامعتبر است
                return null;
            }
        }
    }
}
