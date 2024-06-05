using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using WebRTCChatApp.MessageService.Data;
using WebRTCChatApp.MessageService.Models;
using WebRTCChatApp.MessageService.Repositories.Interfaces;

namespace WebRTCChatApp.MessageService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDbContext _context;
        public MessageRepository(MessageDbContext context)
        {
            _context = context;
        }
        public async Task<bool> SendMessage(Message message)
        {
            _context.Add(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Message>> ReceiveMessages(string receiverId)
        {
            var messages = _context.Messages.Where(m => m.ReceiverId == receiverId);
            return messages;
        }
    }
}
