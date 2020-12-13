using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NotificationCenter.Business.Events;
using NotificationCenter.Hubs;

namespace NotificationCenter.Consumers
{
    public class NotificationSeenStatusChangedInformConsumer : IConsumer<NotificationSeenStatusChangedEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationSeenStatusChangedInformConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<NotificationSeenStatusChangedEvent> context)
        {
            NotificationSeenStatusChangedEvent notificationSeenStatusChangedEvent = context.Message;
            string serializedNotification = JsonConvert.SerializeObject(notificationSeenStatusChangedEvent);

            await _hubContext.Clients.User(notificationSeenStatusChangedEvent.UserId)
                             .SendAsync(NotificationHub.UPDATE_NOTIFICATION_SEEN_STATUS, serializedNotification)
                             .ConfigureAwait(false);
        }
    }
}