using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationCenter.Business.Events;
using NotificationCenter.Hubs;

namespace NotificationCenter.Consumers
{
    public class NotificationCreatedInformConsumer : IConsumer<NotificationCreatedEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationCreatedInformConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<NotificationCreatedEvent> context)
        {
            NotificationCreatedEvent notificationCreatedEvent = context.Message;

            await _hubContext.Clients.User(notificationCreatedEvent.Username)
                             .SendAsync(NotificationHub.SEND_NOTIFICATION, notificationCreatedEvent)
                             .ConfigureAwait(false);
        }
    }
}