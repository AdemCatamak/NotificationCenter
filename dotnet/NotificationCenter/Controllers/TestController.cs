using System.Net;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NotificationCenter.ExternalIntegrationEvents;

namespace NotificationCenter.Controllers
{
    [Route("tests")]
    public class TestController : ControllerBase
    {
        private readonly IBusControl _busControl;

        public TestController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        [HttpPost("order-approved")]
        public async Task<IActionResult> OrderApproved([FromBody] OrderApprovedEvent orderApprovedEvent)
        {
            await _busControl.Publish(orderApprovedEvent);

            return StatusCode((int) HttpStatusCode.Accepted);
        }

        [HttpPost("order-shipped")]
        public async Task<IActionResult> OrderShipped([FromBody] OrderShippedEvent orderShippedEvent)
        {
            await _busControl.Publish(orderShippedEvent);

            return StatusCode((int) HttpStatusCode.Accepted);
        }
    }
}