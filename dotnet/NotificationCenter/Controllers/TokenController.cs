using System.Net;
using Microsoft.AspNetCore.Mvc;
using NotificationCenter.Business.Services;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Controllers
{
    [Route("tokens")]
    public class TokenController : ControllerBase
    {
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public TokenController(IAccessTokenGenerator accessTokenGenerator)
        {
            _accessTokenGenerator = accessTokenGenerator;
        }

        [HttpPost]
        public IActionResult PostToken([FromBody] PostTokenHttpRequest? postTokenHttpRequest)
        {
            Username uName = new Username(postTokenHttpRequest?.Username ?? string.Empty);
            AccessToken accessToken = _accessTokenGenerator.Generate(uName);
            return StatusCode((int) HttpStatusCode.Created, accessToken);
        }

        public class PostTokenHttpRequest
        {
            public string? Username { get; set; }
        }
    }
}