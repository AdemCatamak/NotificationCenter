using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace NotificationCenter.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public const string GET_NOTIFICATION = "GetNotification";
    }
}