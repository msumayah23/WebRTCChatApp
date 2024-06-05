using WebRTCChatApp.MessageService.Models;

namespace WebRTCChatApp.MessageService.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> ReceiveMessage(string receiverId);
        Task<bool> SendMessage(Message message);
    }
}