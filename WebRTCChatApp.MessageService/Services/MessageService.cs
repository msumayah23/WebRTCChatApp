using Microsoft.Identity.Client;
using WebRTCChatApp.MessageService.Models;
using WebRTCChatApp.MessageService.Repositories.Interfaces;

namespace WebRTCChatApp.MessageService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<bool> SendMessage(Message message)
        {
            return await _messageRepository.SendMessage(message);
        }
        public async Task<IEnumerable<Message>> ReceiveMessage(string receiverId)
        {
            return await _messageRepository.ReceiveMessages(receiverId);
        }
    }
}
