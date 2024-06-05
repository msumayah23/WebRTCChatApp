using WebRTCChatApp.SignalService.Models;
using WebRTCChatApp.SignalService.Repositories.Interfaces;

namespace WebRTCChatApp.SignalService.Services
{
    public class SignalService : ISignalService
    {
        private readonly ISignalRepository _signallingRepository;
        public SignalService(ISignalRepository signallingRepository)
        {
            _signallingRepository = signallingRepository;
        }
        public Task<IEnumerable<Signal>> ReceiveSignal(string receiverId)
        {
            var signalList = _signallingRepository.ReceiveSignal(receiverId);
            return signalList;
        }
        public Task<bool> SendSignal(Signal signal)
        {
            return _signallingRepository.SendSignal(signal);
        }
    }
}
