using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
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

            var notificationSeenStatusChangedResponse = new
                    {
                        notificationSeenStatusChangedEvent.CorrelationId,
                        notificationSeenStatusChangedEvent.IsSeen
                    };

            await _hubContext.Clients.User(notificationSeenStatusChangedEvent.Username)
                             .SendAsync(NotificationHub.NOTIFICATION_SEEN_STATUS_CHANGED, notificationSeenStatusChangedResponse)
                             .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}