using Microsoft.AspNetCore.SignalR;

namespace NotificationCenter.Hubs
{
    public class NotificationHub : Hub
    {
        public const string GET_NOTIFICATION = "GetNotification";
        public const string UPDATE_NOTIFICATION_SEEN_STATUS = "UpdateNotificationSeenStatus";
    }
}