using CompanyProfile.Models;
using CompanyProfile.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProfile.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public string generateTokenString(Company user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user.Email), "User email cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentNullException(nameof(user.Username), "User username cannot be null or empty.");
            }
            var key = _config.GetSection("Jwt:Key").Value;
            var issuer = _config.GetSection("Jwt:Issuer").Value;
            var audience = _config.GetSection("Jwt:Audience").Value;

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "JWT Key cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(issuer))
            {
                throw new ArgumentNullException(nameof(issuer), "JWT Issuer cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(audience))
            {
                throw new ArgumentNullException(nameof(audience), "JWT Audience cannot be null or empty.");
            }
            Console.WriteLine($"JWT Key: {key}, Issuer: {issuer}, Audience: {audience}");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "CompanyAdmin")
            };

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: issuer,
                audience: audience,
                signingCredentials: signingCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;
        }

    }
}
