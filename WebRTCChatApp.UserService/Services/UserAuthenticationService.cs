using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.UserService.Services.Interfaces;

namespace WebRTCChatApp.UserService.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private IConfiguration _config;
        public UserAuthenticationService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> GenerateToken()
        {
            //generate token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(1200),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
}
}
