using WebRTCChatApp.UserService.Models;

namespace WebRTCChatApp.UserService.Repositories.Interfaces
{
    public interface IUserManagementRepository
    {
        Task<bool> AddUser(UserDto user, CancellationToken cancellationToken = default);
        Task<bool> DeleteUser(int id, CancellationToken cancellationToken = default);
        Task<User?> GetUserById(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>?> GetUsers(CancellationToken cancellationToken = default);
        Task<bool> UpdateUser(int id, User user, CancellationToken cancellationToken = default);
    }
}