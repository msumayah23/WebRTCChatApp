using Microsoft.AspNetCore.Mvc;
using Moq;
using WebRTCChatApp.SignalService.Controllers;
using WebRTCChatApp.SignalService.Models;
using WebRTCChatApp.SignalService.Repositories.Interfaces;
using Xunit;

namespace WebRTCChatApp.SignalService.Tests
{
    public class SignalControllerTests
    {
        private readonly SignalController _controller;
        private readonly Mock<ISignalRepository> _mockRepo;
        private readonly Mock<WebRtcHub> _hub;

        public SignalControllerTests()
        {
            _mockRepo = new Mock<ISignalRepository>();
            _controller = new SignalController(_mockRepo.Object, _hub.Object);
        }

        [Fact]
        public void SendSignal_ReturnsOkResult()
        {
            // Arrange
            var signal = new Signal
            {
                SenderId = "user1",
                ReceiverId = "user2",
                Data = "test signal"
            };

            // Act
            var result = _controller.SendSignalAsync(signal);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(repo => repo.SendSignal(signal), Times.Once);
        }

        [Fact]
        public async Task ReceiveSignal_ReturnsOkResultWithSignals()
        {
            // Arrange
            var receiverId = "user2";
            var signals = new List<Signal>
                                    {
                                        new Signal { SenderId = "user1", ReceiverId = "user2", Data = "signal 1" },
                                        new Signal { SenderId = "user3", ReceiverId = "user2", Data = "signal 2" }
                                    }.AsEnumerable();
            _mockRepo.Setup(repo => repo.ReceiveSignal(receiverId)).ReturnsAsync(signals);

            // Act
            var result = _controller.ReceiveSignal(receiverId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var signalsTask = Assert.IsType<Task<IEnumerable<Signal>>>(okResult.Value);
            var signalsEnumerable = await signalsTask;
            var signalsList = signalsEnumerable.ToList();
            Assert.IsType<List<Signal>>(signalsList);
        }
    }
}
