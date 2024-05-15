﻿using CompanyProfile.Models;
using CompanyProfile.Models.Authentication.Login;
using CompanyProfile.Repository;
using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public AuthenticationController(UserManager<IdentityUser> userManager,
                               SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _companyRepository = companyRepository;
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

                    if (identityUser != null)
                    {
                        int code = 200;
                        string message = "Login successful";
                        return StatusCode(code, new { Code = code, Message = message });
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