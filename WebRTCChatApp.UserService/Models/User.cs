using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebRTCChatApp.UserService.Models
{
    public class User:IdentityUser
    {
    }
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }  
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
