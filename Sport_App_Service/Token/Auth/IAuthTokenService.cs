using System.Security.Claims;

namespace Sports_App_Service.Token.Auth
{
    public interface IAuthTokenService
    {
        string GenerateToken(int userId, string username, string email, string role);
        ClaimsPrincipal? ValidateToken(string token);
        string? GetClaim(string token, string claimType);
    }
}
