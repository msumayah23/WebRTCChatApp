using Microsoft.EntityFrameworkCore;
using WebRTCChatApp.SignalService.Models;

namespace WebRTCChatApp.SignalService.Data
{
    public class SignalDbContext : DbContext
    {
        public SignalDbContext(DbContextOptions<SignalDbContext> options) : base(options)
        {
        }
        public DbSet<Signal> Signals { get; set; }
    }
}
