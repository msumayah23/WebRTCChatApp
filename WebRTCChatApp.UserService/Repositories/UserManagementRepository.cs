using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebRTCChatApp.UserService.Data;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.UserService.Repositories.Interfaces;

namespace WebRTCChatApp.UserService.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<UserManagementRepository> _logger;
        public UserManagementRepository(UserDbContext context, ILogger<UserManagementRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Retrieve all users
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// 
        public async Task<IEnumerable<User>?> GetUsers(CancellationToken cancellationToken=default)
        {
            try
            {
                var users = await _context.Users.ToListAsync(cancellationToken);
                Log.Information("Users info: " + users);
                return users;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving the users.");
                return null;
            }
        }
        /// <summary>
        /// Get User by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<User?> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _context.Users.FindAsync(id, cancellationToken);
                Log.Information("User info: " + user);
                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving the user.");
                return null;
            }
        }
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> AddUser(UserDto user, CancellationToken cancellationToken=default)
        {
            try
            {
                // Check if a user with the same username already exists
                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == user.UserName, cancellationToken);
                if (existingUser != null)
                {                 
                    _logger.LogInformation("A user with this username already exists.");
                    return false;
                }
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = user.UserName,
                    NormalizedUserName = user.UserName.ToUpper(),
                    Email = user.Email,
                    NormalizedEmail = user.Email.ToUpper(),
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var passwordHasher = new PasswordHasher<User>();
                newUser.PasswordHash = passwordHasher.HashPassword(newUser, user.Password);

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding the user.");
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
        public async Task<bool> UpdateUser(int id, User user, CancellationToken cancellationToken=default)
        {
            try
            { 
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating the user with ID {UserId}", id);
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
                var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken=default);
                if (user == null)
                {
                    return false;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the user with ID {UserId}", id);
                return false;
            }
        }
    }
}
