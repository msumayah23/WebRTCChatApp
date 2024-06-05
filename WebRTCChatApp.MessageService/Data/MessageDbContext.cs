using Microsoft.EntityFrameworkCore;
using WebRTCChatApp.MessageService.Models;

namespace WebRTCChatApp.MessageService.Data
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
        {
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
    }
}
