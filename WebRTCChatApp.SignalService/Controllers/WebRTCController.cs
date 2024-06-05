using Microsoft.AspNetCore.Mvc;
using SIPSorcery.Net;
using WebRTCChatApp.SignalService.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebRTCChatApp.SignalService.Controllers
{
    [Route("api/webrtc")]
    [ApiController]
    public class WebRTCController : ControllerBase
    {
        private readonly ILogger<WebRTCController> _logger;
        private readonly WebRTCHostedService _webRTCServer;

        public WebRTCController(ILogger<WebRTCController> logger, WebRTCHostedService webRTCServer)
        {
            _logger = logger;
            _webRTCServer = webRTCServer;
        }

        [HttpGet]
        [Route("getoffer")]
        public async Task<IActionResult> GetOffer(string id)
        {
            _logger.LogDebug($"WebRTCController GetOffer {id}.");
            return Ok(await _webRTCServer.GetOffer(id));
        }

        [HttpPost]
        [Route("setanswer")]
        public IActionResult SetAnswer(string id, [FromBody] RTCSessionDescriptionInit answer)
        {
            _logger.LogDebug($"SetAnswer {id} {answer?.type} {answer?.sdp}.");

            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("The id cannot be empty in SetAnswer.");
            }
            else if (string.IsNullOrWhiteSpace(answer?.sdp))
            {
                return BadRequest("The SDP answer cannot be empty in SetAnswer.");
            }

            _webRTCServer.SetRemoteDescription(id, answer);
            return Ok();
        }

        [HttpPost]
        [Route("addicecandidate")]
        public IActionResult AddIceCandidate(string id, [FromBody] RTCIceCandidateInit iceCandidate)
        {
            _logger.LogDebug($"SetIceCandidate {id} {iceCandidate?.candidate}.");

            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("The id cannot be empty in AddIceCandidate.");
            }
            else if (string.IsNullOrWhiteSpace(iceCandidate?.candidate))
            {
                return BadRequest("The candidate field cannot be empty in AddIceCandidate.");
            }

            _webRTCServer.AddIceCandidate(id, iceCandidate);

            return Ok();
        }
    }
}
