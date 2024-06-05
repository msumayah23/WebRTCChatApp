using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRTCChatApp.MessageService.Controllers;
using WebRTCChatApp.MessageService.Services;
using WebRTCChatApp.MessageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebRTCChatApp.MessageService.UnitTest.Controllers
{
    public class MessageControllerTests
    {
        [Fact]
        public void SendMessage_ValidMessage_ReturnsOkResult()
        {
            // Arrange
            var messageServiceMock = new Mock<IMessageService>();
            var controller = new MessageController(messageServiceMock.Object);
            var message = new Models.Message { ReceiverId = "receiverId", TextMessage = "Test message" };

            // Act
            var result = controller.SendMessage(message);

            // Assert
            Assert.IsType<OkResult>(result);
            messageServiceMock.Verify(m => m.SendMessage(message), Times.Once);
        }

        [Fact]
        public async Task ReceiveMessages_ReturnsOkResult_WithMessages()
        {
            // Arrange
            var mockMessageService = new Mock<IMessageService>();
            mockMessageService.Setup(x => x.ReceiveMessage(It.IsAny<string>()))
                .ReturnsAsync((string receiverId) =>
                {
                    var messages = new List<Models.Message>
                    {
                new Models.Message { SenderId = "456", ReceiverId = receiverId, TextMessage = "Test message 1", Timestamp = DateTime.UtcNow },
                new Models.Message { SenderId = "789", ReceiverId = receiverId, TextMessage = "Test message 2", Timestamp = DateTime.UtcNow.AddMinutes(1) }
                    };
                    return messages.AsEnumerable();
                });

            var controller = new MessageController(mockMessageService.Object);
            var receiverId = "456";

            // Act
            var result = await controller.ReceiveMessages(receiverId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messages = Assert.IsAssignableFrom<IEnumerable<Models.Message>>(okResult.Value);
            Assert.Equal(2, messages.Count());
        }
    }
}