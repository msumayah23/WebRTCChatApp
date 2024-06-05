using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.UserService.Services.Interfaces;

namespace WebRTCChatApp.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserAuthenticationService _authenticationService;
        private readonly IConfiguration _config;

        public UserAuthenticationController(IConfiguration config, SignInManager<User> signInManager, UserManager<User> userManager, IUserAuthenticationService authenticationService)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _authenticationService = authenticationService;
        }
  

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                Log.Information("Response" + result.ToString());
                if (result.Succeeded)
                {
                    var token= _authenticationService.GenerateToken();
                    return Ok(token);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error("Response" + ex.Message);
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
