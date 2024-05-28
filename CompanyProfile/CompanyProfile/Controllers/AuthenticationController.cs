using CompanyProfile.Models;
using CompanyProfile.Models.Authentication.Login;
using CompanyProfile.Repository;
using CompanyProfile.Services;
using FluentMigrator.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedContent.Messages;

namespace CompanyProfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAuthService _authService;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuthenticationController(UserManager<IdentityUser> userManager,
                               SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ICompanyRepository companyRepository, IAuthService authService, IPublishEndpoint publishEndpoint)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _companyRepository = companyRepository;
            _authService = authService;
            _publishEndpoint = publishEndpoint;
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Models.Authentication.Signup.RegisterCompany model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return StatusCode(409, new { Code = 409, Message = "Email is already registered. Please login or use a different email." });
                }
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "CompanyAdmin");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var company = new Company();
                    company.Email = user.Email;
                    company.Username = model.CompanyName;
                    await _companyRepository.AddCompanyAsync(company);

                    int code = 200;
                    string message = "Registration successful";
                    return StatusCode(code, new { Code = code, Message = message });
                }
                var errorMessage = "Registration failed. Errors: ";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorMessage += $"{error.Description}. ";
                }
                return StatusCode(400, new { Code = 400, Message = errorMessage });
            }
            return StatusCode(400, new { Code = 400, Message = "Registration failed. Errors:" });
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginCompany user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    var identityUser = await _userManager.FindByEmailAsync(user.Email);
                    string name=await _companyRepository.createUserCookie(user.Email);
                    var company = new Company
                    {
                        Email = user.Email,
                        Username = name,
                    };
                    string token=_authService.generateTokenString(company);
                    if (identityUser != null)
                    {
                        var companyJwTokenJobPosting = new CompanyJWTokenJobPosting();
                        companyJwTokenJobPosting.CompanyTokenString = token;
                        await _publishEndpoint.Publish<CompanyJWTokenJobPosting>(companyJwTokenJobPosting);

                        var companyJwTokenApplyForJob = new CompanyJWTokenApplyForJob();
                        companyJwTokenApplyForJob.CompanyTokenString = token;
                        await _publishEndpoint.Publish<CompanyJWTokenApplyForJob>(companyJwTokenApplyForJob);

                        var companyJwTokenResume = new CompanyJWTokenResume();
                        companyJwTokenResume.CompanyTokenString = token;
                        await _publishEndpoint.Publish<CompanyJWTokenResume>(companyJwTokenResume);

                        int code = 200;
                        string message = "Login successful";
                        return StatusCode(code, new { Code = code, Message = message,Token=token });
                    }
                    else
                    {
                        return StatusCode(408, new { Code = 408, Message = "Cookie Not Created" });
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Credentials");
                return StatusCode(409, new { Code = 409, Message = "Invalid Login Attempt" });
            }
            return StatusCode(400, new { Code = 400, Message = "Signin failed. Errors:" });
        }
        [Authorize(Roles = "CompanyAdmin")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            int code = 200;
            string message = "Logout successful";
            return StatusCode(code, new { Code = code, Message = message });
        }
    }
}
