using System.Security.Claims;

namespace Sports_App_Service.Token.Otp
{
    public interface IOtpTokenService
    {
        string GenerateOtpToken(string email);
        ClaimsPrincipal? ValidateToken(string token);
        string? GetClaim(string token, string claimType);
    }
}
