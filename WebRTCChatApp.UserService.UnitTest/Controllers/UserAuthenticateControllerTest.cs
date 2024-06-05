using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRTCChatApp.UserService.Controllers;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.UserService.Services.Interfaces;

namespace WebRTCChatApp.UserService.UnitTest.Controllers
{
    public class UserAuthenticationControllerTest
    {
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IUserAuthenticationService> _mockAuthService;
        private readonly Mock<IConfiguration> _mockConfig;

        public UserAuthenticationControllerTest()
        {
            _mockSignInManager = new Mock<SignInManager<User>>(
                new Mock<UserManager<User>>(
                    new Mock<IUserStore<User>>().Object,
                    null, null, null, null, null, null, null, null
                ).Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null
            );

            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null
            );

            _mockAuthService = new Mock<IUserAuthenticationService>();
            _mockConfig = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WithToken_OnSuccess()
        {
            // Arrange
            var userLoginDto = new UserLoginDto { UserName = "testuser", Password = "Password123!" };
            var mockToken = "mockToken";

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(userLoginDto.UserName, userLoginDto.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockAuthService.Setup(x => x.GenerateToken())
                .ReturnsAsync(mockToken);

            var controller = new UserAuthenticationController(_mockConfig.Object, _mockSignInManager.Object, _mockUserManager.Object, _mockAuthService.Object);

            // Act
            var result = await controller.Login(userLoginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(mockToken, okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorizedResult_OnFailure()
        {
            // Arrange
            var userLoginDto = new UserLoginDto { UserName = "testuser", Password = "wrongPassword" };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(userLoginDto.UserName, userLoginDto.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var controller = new UserAuthenticationController(_mockConfig.Object, _mockSignInManager.Object, _mockUserManager.Object, _mockAuthService.Object);

            // Act
            var result = await controller.Login(userLoginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_OnException()
        {
            // Arrange
            var userLoginDto = new UserLoginDto { UserName = "testuser", Password = "Password123!" };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(userLoginDto.UserName, userLoginDto.Password, false, false))
                .ThrowsAsync(new Exception("Test exception"));

            var controller = new UserAuthenticationController(_mockConfig.Object, _mockSignInManager.Object, _mockUserManager.Object, _mockAuthService.Object);

            // Act
            var result = await controller.Login(userLoginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var badRequestValue = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Equal("Test exception", badRequestValue["Message"]);
        }
    }
}
