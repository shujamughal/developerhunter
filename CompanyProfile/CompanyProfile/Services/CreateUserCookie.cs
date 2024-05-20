using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace CompanyProfile.Services
{
    public class CreateUserCookie:ICreateUserCookie
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateUserCookie(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public bool SetUserCookie(string email,string name)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext.Request.Cookies.TryGetValue("Company", out var existingEmail))
                {
                    httpContext.Response.Cookies.Delete("Company");
                }
                var cookieValues = new Dictionary<string, string>
                {
                    { "Email", email },
                    { "CompanyName", name }
                };
                httpContext.Response.Cookies.Append("Company", JsonConvert.SerializeObject(cookieValues), new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(10),
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return false;
            }
        }
    }
}
