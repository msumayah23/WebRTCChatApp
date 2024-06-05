namespace WebRTCChatApp.MessageService.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Message> Messages { get; set; }
    }
}
