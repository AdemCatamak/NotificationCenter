using Microsoft.AspNetCore.SignalR;

namespace NotificationCenter.Hubs
{
    public class NotificationHub : Hub
    {
        public const string GET_NOTIFICATION = "GetNotification";
    }
}