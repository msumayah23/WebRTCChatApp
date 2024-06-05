using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("AuthenticationService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7063/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpClient("SignalingService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7057/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

//Microsoft Logging Ext
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// SignalR service
builder.Services.AddSignalR();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//Js and HTML static files
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

