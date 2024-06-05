using Serilog;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.UserService.Repositories.Interfaces;
using WebRTCChatApp.UserService.Services.Interfaces;

namespace WebRTCChatApp.UserService.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;
        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }
       /// <summary>
       /// Retrieve all user details
       /// </summary>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
       public async Task<IEnumerable<User>?> GetUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userManagementRepository.GetUsers(cancellationToken);
                Log.Information("Users info: " + users);
                if (users == null)
                {
                    Log.Information("No users available.");
                }
                return users;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving the users.");
                return null;
            }
        }
        /// <summary>
        /// Get User details by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserDto?> GetUserById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManagementRepository.GetUserById(id, cancellationToken);
                if (user == null)
                {
                    Log.Information("User not found.");
                    return null;
                }
                //var passwordHash = user.PasswordHash != null ? Convert.ToBase64String(user.PasswordHash) : string.Empty;
                //string passwordValue = "-";
                //if (passwordHash != null)
                //{
                //    passwordValue = Convert.ToHexString(passwordHash);
                //}
                var userDto = new UserDto
                {
                    UserName = user.UserName??"-"
                   // Password = passwordValue
                };
                return userDto;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving the user.");
                return null;
            }
        }
       /// <summary>
       /// Save new user
       /// </summary>
       /// <param name="user"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
       public async Task<bool> AddUser(UserDto user, CancellationToken cancellationToken)  
        {
            try
            {
                var isSuccess = await _userManagementRepository.AddUser(user, cancellationToken);
                return isSuccess;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while saving the user.");
                return false;
            }
        }
        /// <summary>
        /// Save updated user details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool > UpdateUser(int id, User user, CancellationToken cancellationToken)
        {
            try
            {
                var isSuccess = await _userManagementRepository.UpdateUser(id, user, cancellationToken);
                return isSuccess;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating information of the user.");
                return false;
            }
        }
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(int id, CancellationToken cancellationToken)
        {
            try
            {
                var isSuccess = await _userManagementRepository.DeleteUser(id, cancellationToken);
                return isSuccess;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while removing the user.");
                return false;
            }
        }
    }
}
