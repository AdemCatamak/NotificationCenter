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

            await _hubContext.Clients.All
                             .SendAsync(NotificationHub.UPDATE_NOTIFICATION_SEEN_STATUS, notificationSeenStatusChangedEvent)
                             .ConfigureAwait(false);
        }
    }
}