using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Sports_App_Model.Setup;

namespace Sports_App_Service.Token.Otp
{
    public class OtpTokenService : IOtpTokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public OtpTokenService(JwtSettings jwtSettings)
        {
            _key = jwtSettings.Key;
            _issuer = jwtSettings.Issuer;
            _audience = jwtSettings.Audience;
        }

        public string GenerateOtpToken(string email)
        {
            var claims = new[]
            {
                new Claim("email", email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string? GetClaim(string token, string claimType)
        {
            var principal = ValidateToken(token);
            var claim = principal?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            return claim ?? null;
        }
    }
}
