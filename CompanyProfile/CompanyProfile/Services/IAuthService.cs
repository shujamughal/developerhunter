using CompanyProfile.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProfile.Services
{
    public interface IAuthService
    {
        string generateTokenString(Company user);
    }
}