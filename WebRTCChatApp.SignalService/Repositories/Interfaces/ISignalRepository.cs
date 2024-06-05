using WebRTCChatApp.SignalService.Models;

namespace WebRTCChatApp.SignalService.Repositories.Interfaces
{
    public interface ISignalRepository
    {
        Task<IEnumerable<Signal>>ReceiveSignal(string receiverId);
        Task<bool> SendSignal(Signal signal);
    }
}