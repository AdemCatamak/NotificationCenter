using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationCenter.Business.Commands;
using NotificationCenter.Business.Services;
using NotificationCenter.Controllers.Requests;
using NotificationCenter.Model.Exceptions;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Controllers
{
    [Authorize]
    [Route("users/{username}/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetNotifications([FromRoute] string username, [FromQuery] GetNotificationCollectionHttpRequest? getNotificationCollectionHttpRequest)
        {
            CheckTokenIsValid(username);

            getNotificationCollectionHttpRequest ??= new GetNotificationCollectionHttpRequest();
            Username uName = new Username(username);
            QueryNotificationCommand queryNotificationCommand = new QueryNotificationCommand(uName, getNotificationCollectionHttpRequest.IsSeen, getNotificationCollectionHttpRequest.Skip, getNotificationCollectionHttpRequest.Take);
            List<NotificationResponse> notificationResponseList = await _notificationService.QueryNotificationAsync(queryNotificationCommand, CancellationToken.None);

            return StatusCode((int) HttpStatusCode.OK, notificationResponseList);
        }

        [HttpPut("{notificationCorrelationId}/seen")]
        public async Task<IActionResult> SetNotificationAsSeen([FromRoute] string username, [FromRoute] string notificationCorrelationId)
        {
            CheckTokenIsValid(username);

            Username uName = new Username(username);
            NotificationCorrelationId correlationId = new NotificationCorrelationId(notificationCorrelationId);

            ChangeNotificationSeenStatusCommand changeNotificationSeenStatusCommand = new ChangeNotificationSeenStatusCommand(uName, correlationId, true);
            await _notificationService.ChangeNotificationSeenStatusAsync(changeNotificationSeenStatusCommand, CancellationToken.None);

            return StatusCode((int) HttpStatusCode.OK);
        }

        [HttpDelete("{notificationCorrelationId}/seen")]
        public async Task<IActionResult> SetNotificationAsUnseen([FromRoute] string username, [FromRoute] string notificationCorrelationId)
        {
            CheckTokenIsValid(username);

            Username uName = new Username(username);
            NotificationCorrelationId correlationId = new NotificationCorrelationId(notificationCorrelationId);

            ChangeNotificationSeenStatusCommand changeNotificationSeenStatusCommand = new ChangeNotificationSeenStatusCommand(uName, correlationId, false);
            await _notificationService.ChangeNotificationSeenStatusAsync(changeNotificationSeenStatusCommand, CancellationToken.None);

            return StatusCode((int) HttpStatusCode.OK);
        }

        private void CheckTokenIsValid(string username)
        {
            bool valid = HttpContext.User.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == username);

            if (!valid) throw new ForbiddenException();
        }
    }
}