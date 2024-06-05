using Microsoft.EntityFrameworkCore;
using Serilog;
using WebRTCChatApp.MessageService.Data;
using WebRTCChatApp.MessageService.Repositories;
using WebRTCChatApp.MessageService.Repositories.Interfaces;
using WebRTCChatApp.MessageService.Services;
public class Program
{
    static void Main(string[] args)
    {
        // Build configuration          
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            //  .WriteTo.Console()
            .WriteTo.File("messageservicelogs/log-.txt", rollingInterval: Serilog.RollingInterval.Day)
            .CreateLogger();
        try
        {
            Log.Information("Message Sevice Application Starting Up");
            // Your application logic here
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Message Sevice Application terminated unexpectedly");
            Console.WriteLine(ex);
        }
        finally
        {
            Log.CloseAndFlush();
        }
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddDbContext<MessageDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();

        //add SignalR Service
        builder.Services.AddSignalR();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //app..UseCors("CorsPolicy");

        app.UseRouting();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapHub<ChatHub>("/chatHub");
        //});

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
