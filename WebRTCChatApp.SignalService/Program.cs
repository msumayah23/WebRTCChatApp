using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using Serilog.Extensions.Logging;
using SIPSorcery.Media;
using SIPSorcery.Net;
using SIPSorceryMedia.Encoders;
using System.Net;
using WebRTCChatApp.SignalService;
using WebRTCChatApp.SignalService.Data;
using WebRTCChatApp.SignalService.Repositories;
using WebRTCChatApp.SignalService.Repositories.Interfaces;
using WebRTCChatApp.SignalService.Services;
using WebSocketSharp.Server;

public class Program
{
    private const int WEBSOCKET_PORT = 8081;
    private const string STUN_URL = "stun:stun.sipsorcery.com";

    private static Microsoft.Extensions.Logging.ILogger logger = NullLogger.Instance;
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
            .WriteTo.File("signalservicelogs/log-.txt", rollingInterval: Serilog.RollingInterval.Day)
            .CreateLogger();
        try
        {
            Log.Information("Signal Sevice Application Starting Up");
            // Start web socket.
            //Console.WriteLine("Starting web socket server...");
            //var webSocketServer = new WebSocketServer(IPAddress.Any, WEBSOCKET_PORT);
            //webSocketServer.AddWebSocketService<WebRTCWebSocketPeer>("/", (peer) => peer.CreatePeerConnection = CreatePeerConnection);
            //webSocketServer.Start();

            //Console.WriteLine($"Waiting for web socket connections on {webSocketServer.Address}:{webSocketServer.Port}...");
            //Console.WriteLine("Press ctrl-c to exit.");

            //// Ctrl-c will gracefully exit the call at any point.
            //ManualResetEvent exitMre = new ManualResetEvent(false);
            //Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            //{
            //    e.Cancel = true;
            //    exitMre.Set();
            //};

            //// Wait for a signal saying the call failed, was cancelled with ctrl-c or completed.
            //exitMre.WaitOne();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Signal Sevice Application terminated unexpectedly");
            Console.WriteLine(ex);
        }
        finally
        {
            Log.CloseAndFlush();
        }
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();
        // Add services to the container.
        builder.Services.AddDbContext<SignalDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<ISignalRepository, SignalRepository>();
        builder.Services.AddScoped<ISignalService, SignalService>();

        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowClient", builder =>
            {
                builder.WithOrigins("https://localhost:7090")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton(typeof(WebRTCHostedService));
        builder.Services.AddHostedService<WebRTCHostedService>();
        builder.Services.AddSignalR();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient("MessageService", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7158/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
  
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("AllowClient");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapHub<ChatHub>("/ChatHub");
        //});
        app.MapHub<WebRtcHub>("webrtchub");
        app.MapHub<ChatHub>("ChatHub");
        app.Run();
    }
    private static Task<RTCPeerConnection> CreatePeerConnection()
    {
        RTCConfiguration config = new RTCConfiguration
        {
            iceServers = new List<RTCIceServer> { new RTCIceServer { urls = STUN_URL } }
        };
        var pc = new RTCPeerConnection(config);

        var testPatternSource = new VideoTestPatternSource();
        var videoEncoderEndPoint = new VideoEncoderEndPoint();
        var audioSource = new AudioExtrasSource(new AudioEncoder(), new AudioSourceOptions { AudioSource = AudioSourcesEnum.Music });

        MediaStreamTrack videoTrack = new MediaStreamTrack(videoEncoderEndPoint.GetVideoSourceFormats(), MediaStreamStatusEnum.SendRecv);
        pc.addTrack(videoTrack);
        MediaStreamTrack audioTrack = new MediaStreamTrack(audioSource.GetAudioSourceFormats(), MediaStreamStatusEnum.SendRecv);
        pc.addTrack(audioTrack);

        testPatternSource.OnVideoSourceRawSample += videoEncoderEndPoint.ExternalVideoSourceRawSample;
        videoEncoderEndPoint.OnVideoSourceEncodedSample += pc.SendVideo;
        audioSource.OnAudioSourceEncodedSample += pc.SendAudio;

        pc.OnVideoFormatsNegotiated += (formats) => videoEncoderEndPoint.SetVideoSourceFormat(formats.First());
        pc.OnAudioFormatsNegotiated += (formats) => audioSource.SetAudioSourceFormat(formats.First());

        pc.onconnectionstatechange += async (state) =>
        {
            logger.LogDebug($"Peer connection state change to {state}.");

            if (state == RTCPeerConnectionState.connected)
            {
                await audioSource.StartAudio();
                await testPatternSource.StartVideo();
            }
            else if (state == RTCPeerConnectionState.failed)
            {
                pc.Close("ice disconnection");
            }
            else if (state == RTCPeerConnectionState.closed)
            {
                await testPatternSource.CloseVideo();
                await audioSource.CloseAudio();
            }
        };

        // Diagnostics.
        pc.OnReceiveReport += (re, media, rr) => logger.LogDebug($"RTCP Receive for {media} from {re}\n{rr.GetDebugSummary()}");
        pc.OnSendReport += (media, sr) => logger.LogDebug($"RTCP Send for {media}\n{sr.GetDebugSummary()}");
        pc.GetRtpChannel().OnStunMessageReceived += (msg, ep, isRelay) => logger.LogDebug($"STUN {msg.Header.MessageType} received from {ep}.");
        pc.oniceconnectionstatechange += (state) => logger.LogDebug($"ICE connection state change to {state}.");

        // To test closing.
        //_ = Task.Run(async () => 
        //{ 
        //    await Task.Delay(5000);

        //    audioSource.OnAudioSourceEncodedSample -= pc.SendAudio;
        //    videoEncoderEndPoint.OnVideoSourceEncodedSample -= pc.SendVideo;

        //    logger.LogDebug("Closing peer connection.");
        //    pc.Close("normal");
        //});

        return Task.FromResult(pc);
    }

    /// <summary>
    ///  Adds a console logger. Can be omitted if internal SIPSorcery debug and warning messages are not required.
    /// </summary>
    private static Microsoft.Extensions.Logging.ILogger AddConsoleLogger()
    {
        var seriLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug)
            .WriteTo.Console()
            .CreateLogger();
        var factory = new SerilogLoggerFactory(seriLogger);
        SIPSorcery.LogFactory.Set(factory);
        return factory.CreateLogger<Program>();
    }
}
