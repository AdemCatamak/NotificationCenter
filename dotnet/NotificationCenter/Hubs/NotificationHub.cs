using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationCenter.Business.Commands;

namespace NotificationCenter.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public const string NOTIFICATION_CREATED = "NotificationReceived";
        public const string NOTIFICATION_SEEN_STATUS_CHANGED = "NotificationSeenStatusChanged";

        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        private string GetUsername()
        {
            string username = Context.UserIdentifier;
            return username;
        }

        public override Task OnConnectedAsync()
        {
            string username = GetUsername();
            _logger.LogInformation($"{username} is connected. #{Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string username = GetUsername();
            if (exception == null)
                _logger.LogInformation($"{username} is disconnected. #{Context.ConnectionId}");
            else
            {
                _logger.LogError($"{username} is disconnected. #{Context.ConnectionId} [Ex: {exception}]");
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}