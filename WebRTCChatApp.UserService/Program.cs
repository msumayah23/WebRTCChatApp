using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using WebRTCChatApp.UserService.Data;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.UserService.Repositories;
using WebRTCChatApp.UserService.Repositories.Interfaces;
using WebRTCChatApp.UserService.Services;
using WebRTCChatApp.UserService.Services.Interfaces;
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
            .WriteTo.File("userservicelogs/log-.txt", rollingInterval: Serilog.RollingInterval.Day)
            .CreateLogger();
        try
        {
            Log.Information("User Sevice Application Starting Up");
            // Your application logic here
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "User Sevice Application terminated unexpectedly");
            Console.WriteLine(ex);
        }
        finally
        {
            Log.CloseAndFlush();
        }
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        // Register IHttpClientFactory
        builder.Services.AddHttpClient();

        builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        builder.Services.AddScoped<IUserManagementService, UserManagementService>();
        builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
        var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = jwtIssuer,
                 ValidAudience = jwtIssuer,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
             };
         });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}
