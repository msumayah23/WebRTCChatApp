using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using WebRTCChatApp.WebApplication.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net.Http;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.SignalService.Models;

namespace WebRTCChatApp.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IHttpClientFactory httpClientFactory,ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Home/Login
        public IActionResult _Login()
        {
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        public async Task<IActionResult> _LoginAsync([FromForm]UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var authenticationClient = _httpClientFactory.CreateClient("AuthenticationService");
                    UserLoginDto userLoginDto = new UserLoginDto()
                    {
                        UserName=model.LoginUserName,
                        Password=model.LoginPassword
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(userLoginDto), Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Post, "api/UserAuthentication/Login")
                    {
                        Content = content
                    };

                    _logger.LogInformation("Sending request to {url}", request.RequestUri);
                    HttpResponseMessage response = await authenticationClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation("Received successful response: {response}", result);

                        return RedirectToAction("Chat", "Home",new { username = model.LoginUserName });
                    }

                    _logger.LogWarning("Received non-success status code: {statusCode}", response.StatusCode);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Error sending request to {url}", "api/UserAuthentication/Login");
                    ModelState.AddModelError(string.Empty, "There was an error connecting to the authentication service.");
                }
            }
                return View(model);
        }

        // GET: Home/Register
        public IActionResult _Register()
        {
            return View();
        }

        // POST: Home/Register
        [HttpPost]
        public async Task<IActionResult> _RegisterAsync([FromForm] UserRegisterViewModel model,CancellationToken cancellationToken=default)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var authenticationClient = _httpClientFactory.CreateClient("AuthenticationService");
                    UserDto userLoginDto = new UserDto()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        Email=model.Email
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(userLoginDto), Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Post, "api/UserManagement/AddUser")
                    {
                        Content = content,
                    };

                    _logger.LogInformation("Sending request to {url}", request.RequestUri);
                    HttpResponseMessage response = await authenticationClient.SendAsync(request,cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation("Received successful response: {response}", result);

                        return RedirectToAction("Chat", "Home", new { username = model.UserName });
                    }

                    _logger.LogWarning("Received non-success status code: {statusCode}", response.StatusCode);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Error sending request to {url}", "api/UserAuthentication/Login");
                    ModelState.AddModelError(string.Empty, "There was an error connecting to the authentication service.");
                }
            }
            //Log.Information("User registration details are invalid.");
            return View(model);
        }
        public IActionResult Chat(string username)
        {
            ViewBag.Username = username;
            return View();
        }
    }
}
