using WebRTCChatApp.MessageService.Models;

namespace WebRTCChatApp.MessageService.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> ReceiveMessages(string receiverId);
        Task<bool> SendMessage(Message message);
    }
}