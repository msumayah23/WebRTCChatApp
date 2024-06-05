using WebRTCChatApp.SignalService.Models;

namespace WebRTCChatApp.SignalService.Services
{
    public interface ISignalService
    {
        Task<IEnumerable<Signal>> ReceiveSignal(string receiverId);
        Task<bool> SendSignal(Signal signal);
    }
}