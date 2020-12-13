using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
            string serializedNotification = JsonConvert.SerializeObject(notificationCreatedEvent);

            await _hubContext.Clients.User(notificationCreatedEvent.UserId)
                             .SendAsync(NotificationHub.GET_NOTIFICATION, serializedNotification)
                             .ConfigureAwait(false);
        }
    }
}