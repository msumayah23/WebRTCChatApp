namespace WebRTCChatApp.SignalService.Models
{
    public class Signal
    {   
        public int Id {  get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
