using Microsoft.AspNetCore.Mvc;
using WebRTCChatApp.MessageService.Models;
using WebRTCChatApp.MessageService.Services;

namespace WebRTCChatApp.MessageService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] Message message)
        {
            message.Timestamp = DateTime.UtcNow;
           _messageService.SendMessage(message);
            return Ok();
        }

        [HttpGet("receive/{receiverId}")]
        public async Task<IActionResult> ReceiveMessages(string receiverId)
        {
            var messages = await _messageService.ReceiveMessage(receiverId);
            return Ok(messages);
        }
    }

}
