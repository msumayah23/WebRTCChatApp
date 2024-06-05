using Microsoft.AspNetCore.SignalR;

namespace WebRTCChatApp.SignalService
{
 
    public class WebRtcHub : Hub
    {
    public async Task JoinChatRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserJoined", Context.ConnectionId);
        }
        public async Task StartCall(IHubContext<WebRtcHub> hubContext, string targetConnectionId)
        {
            await Clients.Client(targetConnectionId).SendAsync("StartCall", Context.ConnectionId);
        }
        public async Task SendSignal(IHubContext<WebRtcHub> hubContext, string targetConnectionId, string signal)
        {
            await hubContext.Clients.Client(targetConnectionId).SendAsync("ReceiveSignal", Context.ConnectionId, signal);
        }
        public async Task SendSdpOffer(IHubContext<WebRtcHub> hubContext, string targetConnectionId, string sdpOffer)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveSdpOffer", Context.ConnectionId, sdpOffer);
        }
        public async Task SendSdpAnswer(IHubContext<WebRtcHub> hubContext, string targetConnectionId, string sdpAnswer)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveSdpAnswer", Context.ConnectionId, sdpAnswer);
        }
        public async Task SendIceCandidate(IHubContext<WebRtcHub> hubContext, string targetConnectionId, string iceCandidate)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, iceCandidate);
        }
        public async Task LeaveChatRoom(IHubContext<WebRtcHub> hubContext, string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserLeft", Context.ConnectionId);
        }
    }
}