using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Jobverse.Utils
{
    public static class TokenManager
    {
        public static string TokenString { get; set; }
        public static string CompanyToken { get; set; } = null;
        public static (string name,string email) getCredentials()
        {
            if (CompanyToken != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(CompanyToken);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                return (emailClaim, nameClaim);
            }
            return (string.Empty,string.Empty);
        }
    }
}
