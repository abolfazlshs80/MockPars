using MockPars.Domain.Models;
using System.Security.Claims;

namespace MockPars.Infrastructure.Service.Jwt;

public interface IJwtService
{
    public string GenerateJwtToken(User user);

    Task<ClaimsPrincipal?> ValidateAndExtractClaimsAsync(string token);
}