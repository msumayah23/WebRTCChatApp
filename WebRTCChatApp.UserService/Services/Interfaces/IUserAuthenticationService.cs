namespace WebRTCChatApp.UserService.Services.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<string> GenerateToken();
    }
}