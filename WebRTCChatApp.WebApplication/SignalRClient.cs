using Microsoft.AspNetCore.SignalR.Client;

namespace WebRTCChatApp.WebApplication
{
    public class SignalRClient
    {
        private readonly HubConnection _connection;

        public SignalRClient(string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                // Handle received message
            });
        }

        public async Task StartConnectionAsync()
        {
            await _connection.StartAsync();
        }

        public async Task SendMessageAsync(string user, string message)
        {
            await _connection.InvokeAsync("SendMessage", user, message);
        }
    }
}
