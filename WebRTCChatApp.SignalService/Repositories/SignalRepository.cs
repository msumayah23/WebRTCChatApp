using Microsoft.AspNetCore.Mvc;
using WebRTCChatApp.SignalService.Data;
using WebRTCChatApp.SignalService.Models;
using WebRTCChatApp.SignalService.Repositories.Interfaces;

namespace WebRTCChatApp.SignalService.Repositories
{
    public class SignalRepository : ISignalRepository
    {
        private readonly SignalDbContext _context;
        public SignalRepository(SignalDbContext signalDbContext)
        {
            _context = signalDbContext;
        }
        public async Task<bool> SendSignal(Signal signal)
        {
            _context.Signals.Add(signal);
            var response= await _context.SaveChangesAsync();
            // Logic to deliver the signal to the receiver can be implemented here.
            return true;
        }

        [HttpGet("receive/{receiverId}")]
        public async Task<IEnumerable<Signal>> ReceiveSignal(string receiverId)
        {
            var signals = _context.Signals.Where(s => s.ReceiverId == receiverId).ToList();
            return signals;
        }
    }
}
