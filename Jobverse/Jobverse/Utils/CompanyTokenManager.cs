using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Jobverse.Utils
{
    public static class CompanyTokenManager
    {
        public static string CompanyTokenString { get; set; }
        public static (string name, string email) getCredentials()
        {
            if (CompanyTokenString != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(CompanyTokenString);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                return (emailClaim, nameClaim);
            }
            return (string.Empty, string.Empty);
        }
    }
}
