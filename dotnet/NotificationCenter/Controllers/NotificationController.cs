using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationCenter.Business.Commands;
using NotificationCenter.Business.Services;
using NotificationCenter.Controllers.Requests;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Controllers
{
    [Route("users/{userId}/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetNotifications([FromRoute] string userId, [FromQuery] GetNotificationCollectionHttpRequest? getNotificationCollectionHttpRequest)
        {
            getNotificationCollectionHttpRequest ??= new GetNotificationCollectionHttpRequest();
            UserId uId = new UserId(userId);
            QueryNotificationCommand queryNotificationCommand = new QueryNotificationCommand(uId, getNotificationCollectionHttpRequest.IsSeen, getNotificationCollectionHttpRequest.Skip, getNotificationCollectionHttpRequest.Take);
            List<NotificationResponse> notificationResponseList = await _notificationService.QueryNotificationAsync(queryNotificationCommand, CancellationToken.None);

            return StatusCode((int) HttpStatusCode.OK, notificationResponseList);
        }

        [HttpPut("{notificationCorrelationId}/seen")]
        public async Task<IActionResult> SetNotificationAsSeen([FromRoute] string userId, [FromRoute] string notificationCorrelationId)
        {
            UserId uId = new UserId(userId);
            NotificationCorrelationId correlationId = new NotificationCorrelationId(notificationCorrelationId);

            ChangeNotificationSeenStatusCommand changeNotificationSeenStatusCommand = new ChangeNotificationSeenStatusCommand(uId, correlationId, true);
            await _notificationService.ChangeNotificationSeenStatusAsync(changeNotificationSeenStatusCommand, CancellationToken.None);

            return StatusCode((int) HttpStatusCode.OK);
        }

        [HttpDelete("{notificationCorrelationId}/seen")]
        public async Task<IActionResult> SetNotificationAsUnseen([FromRoute] string userId, [FromRoute] string notificationCorrelationId)
        {
            UserId uId = new UserId(userId);
            NotificationCorrelationId correlationId = new NotificationCorrelationId(notificationCorrelationId);

            ChangeNotificationSeenStatusCommand changeNotificationSeenStatusCommand = new ChangeNotificationSeenStatusCommand(uId, correlationId, false);
            await _notificationService.ChangeNotificationSeenStatusAsync(changeNotificationSeenStatusCommand, CancellationToken.None);

            return StatusCode((int) HttpStatusCode.OK);
        }
    }
}