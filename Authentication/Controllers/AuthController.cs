using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;  
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUser(user);
                if (result.Succeeded)
                {
                    return Ok("Registration successful");
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(errors);
                }
            }

            return BadRequest("Somthing went wrong!");
           
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(User user)
        {
            if(ModelState.IsValid)
            {
                var result = await _authService.Login(user);
                if (result == true)
                {
                    var tokenString = _authService.generateTokenString(user);
                    return Ok(tokenString);
                }
                return BadRequest("Incorrect password or username");
            }

            return BadRequest("Something went wrong");
        }
    }
}
