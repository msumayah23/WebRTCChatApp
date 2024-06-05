using Microsoft.AspNetCore.SignalR;

namespace WebRTCChatApp.SignalService
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var contaxtDetails = Context;
            await Clients.All.SendAsync("ReceiveMessage", $"{ Context.ConnectionId}"+" has joined.");
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendOffer(string user, string offer)
        {
            await Clients.User(user).SendAsync("ReceiveOffer", offer);
        }

        public async Task SendAnswer(string user, string answer)
        {
            await Clients.User(user).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate(string user, string candidate)
        {
            await Clients.User(user).SendAsync("ReceiveIceCandidate", candidate);
        }
    }
}
