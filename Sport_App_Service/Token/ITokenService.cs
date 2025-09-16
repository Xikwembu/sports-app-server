using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sport_App_Service.Token
{
    public interface ITokenService
    {
        string GenerateToken(string email, string role);
        ClaimsPrincipal? ValidateToken(string token);
        string? GetClaim(string token, string claimType);
    }
}
