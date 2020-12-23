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
        public IActionResult PostToken([FromBody] string? username)
        {
            Username uName = new Username(username ?? string.Empty);
            AccessToken accessToken = _accessTokenGenerator.Generate(uName);
            return StatusCode((int) HttpStatusCode.Created, accessToken);
        }
    }
}