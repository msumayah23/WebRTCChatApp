using WebRTCChatApp.UserService.Models;

namespace WebRTCChatApp.UserService.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<bool> AddUser(UserDto user, CancellationToken cancellationToken);
        Task<bool> DeleteUser(int id, CancellationToken cancellationToken);
        Task<UserDto?> GetUserById(int id, CancellationToken cancellationToken);
        Task<IEnumerable<User>?> GetUsers(CancellationToken cancellationToken);
        Task<bool > UpdateUser(int id, User user, CancellationToken cancellationToken);
    }
}