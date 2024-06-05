using System.ComponentModel.DataAnnotations;

namespace WebRTCChatApp.WebApplication.Models
{
    public class UserRegisterViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

}
