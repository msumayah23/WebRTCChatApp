using Moq;
using WebRTCChatApp.UserService.Controllers;
using WebRTCChatApp.UserService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebRTCChatApp.UserService.Models;

namespace WebRTCChatApp.UserService.UnitTest.Controllers
{
    public class UserManagementControllerTests
    {
        private readonly Mock<IUserManagementService> _mockUserManagementService;
        private readonly UserManagementController _controller;

        public UserManagementControllerTests()
        {
            _mockUserManagementService = new Mock<IUserManagementService>();
            _controller = new UserManagementController(_mockUserManagementService.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsUsersList()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", UserName = "User1" },
                new User { Id = "2",UserName = "User2" }
            };
            _mockUserManagementService.Setup(service => service.GetUsers(It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
            var returnValue = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetUser_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "User1" };
            //_mockUserManagementService.Setup(service => service.GetUserById(1, It.IsAny<CancellationToken>()))
            //                          .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user, returnValue);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            //_mockUserManagementService.Setup(service => service.GetUserById(1, It.IsAny<CancellationToken>()))
            //                          .ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.False(((dynamic)notFoundResult.Value).success);
        }

        [Fact]
        public async Task AddUser_ReturnsSuccess_WhenUserIsAdded()
        {
            // Arrange
            var userDto = new UserDto { UserName = "NewUser" };
            _mockUserManagementService.Setup(service => service.AddUser(userDto, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(true);

            // Act
            var result = await _controller.AddUser(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(((dynamic)okResult.Value).success);
        }

        [Fact]
        public async Task AddUser_ReturnsBadRequest_WhenUserIsNotAdded()
        {
            // Arrange
            var userDto = new UserDto { UserName = "NewUser" };
            _mockUserManagementService.Setup(service => service.AddUser(userDto, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(false);

            // Act
            var result = await _controller.AddUser(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.False(((dynamic)badRequestResult.Value).success);
        }

        [Fact]
        public async Task UpdateUser_ReturnsSuccess_WhenUserIsUpdated()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "UpdatedUser" };
            _mockUserManagementService.Setup(service => service.UpdateUser(1, user, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUser(1, user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(((dynamic)okResult.Value).success);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenUserIsNotUpdated()
        {
            // Arrange
            var user = new User {Id = "1", UserName = "UpdatedUser" };
            _mockUserManagementService.Setup(service => service.UpdateUser(1, user, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUser(1, user);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.False(((dynamic)badRequestResult.Value).success);
        }

        [Fact]
        public async Task DeleteUser_ReturnsSuccess_WhenUserIsDeleted()
        {
            // Arrange
            _mockUserManagementService.Setup(service => service.DeleteUser(1, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(((dynamic)okResult.Value).success);
        }

        [Fact]
        public async Task DeleteUser_ReturnsBadRequest_WhenUserIsNotDeleted()
        {
            // Arrange
            _mockUserManagementService.Setup(service => service.DeleteUser(1, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.False(((dynamic)badRequestResult.Value).success);
        }
    }
}