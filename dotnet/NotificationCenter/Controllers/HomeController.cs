using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace NotificationCenter.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Microsoft.AspNetCore.Components.Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Home()
        {
            return Redirect($"{Request.Scheme}://{Request.Host.ToUriComponent()}/swagger");
        }

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            var response = new {Environment.MachineName};
            return StatusCode((int) HttpStatusCode.OK, response);
        }
    }
}