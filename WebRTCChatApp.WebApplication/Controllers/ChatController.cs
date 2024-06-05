using Microsoft.AspNetCore.Mvc;
using WebRTCChatApp.SignalService.Models;

namespace WebRTCChatApp.WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SignalingService");
        }

        [HttpPost("SendSignal")]
        public async Task<IActionResult> SendSignal([FromBody] Signal signal)
        {
                      //var signalingClient = _httpClientFactory.CreateClient("signalingService");
     //   signal: Id,SenderId,ReceiverId,Type,Data
        //var signalContent = new StringContent(JsonConvert.SerializeObject(userLoginDto), Encoding.UTF8, "application/json");

        //    var signalRequest = new HttpRequestMessage(HttpMethod.Post, "api/Signal/SendSignal")
        //    {
        //        Content = signalContent
        //    };

        //    _logger.LogInformation("Sending request to {url}", request.RequestUri);
        //    HttpResponseMessage signalResponse = await signalingClient.SendAsync(request);

            var response = await _httpClient.PostAsJsonAsync("api/Signal/SendSignal", signal);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            return StatusCode((int)response.StatusCode);
        }
    }
}
