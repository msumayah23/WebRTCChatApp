using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebRTCChatApp.SignalService.Models;
using WebRTCChatApp.SignalService.Repositories.Interfaces;

namespace WebRTCChatApp.SignalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignalController : ControllerBase
    {
        private readonly ISignalRepository _signallingRepository;
        WebRtcHub _hub;
        //private readonly HttpClient _httpClient;
        public SignalController(ISignalRepository signallingRepository, WebRtcHub hub)//, IHttpClientFactory httpClientFactory)
        {
            _signallingRepository = signallingRepository;
            _hub = hub;
            //  _httpClient = httpClientFactory.CreateClient("MessageService");
        }

        [HttpPost("SendSignal")]
        public async Task<IActionResult> SendSignalAsync([FromBody] Signal signal)
        {
            //_hub.JoinChatRoom();
                var result=await _signallingRepository.SendSignal(signal);
                //Deliver the signal to the receiver
                //var request = new HttpRequestMessage(HttpMethod.Post, "api/send")
                //{
                //    Content = new StringContent(JsonConvert.SerializeObject(new { signal }), Encoding.UTF8, "application/json")
                //};

                //var response = await _httpClient.SendAsync(request);
                //if (response.IsSuccessStatusCode)
                //{
                //    var result = await response.Content.ReadAsStringAsync();
                //    return Ok(result);
                //}
                return Ok(result);
            }

                [HttpGet("receive/{receiverId}")]
        public IActionResult ReceiveSignal(string receiverId)
        {
            var signals = _signallingRepository.ReceiveSignal(receiverId);
            return Ok(signals);
        }
    }
}