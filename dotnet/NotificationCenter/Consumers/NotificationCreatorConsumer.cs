using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NotificationCenter.Business.Commands;
using NotificationCenter.Business.Services;
using NotificationCenter.ExternalIntegrationEvents;
using NotificationCenter.Model.Exceptions.Imp;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Consumers
{
    public class NotificationCreatorConsumer : IConsumer<OrderApprovedEvent>
                                             , IConsumer<OrderShippedEvent>
    {
        private readonly INotificationService _notificationService;

        public NotificationCreatorConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<OrderApprovedEvent> context)
        {
            OrderApprovedEvent orderApprovedEvent = context.Message;

            NotificationCorrelationId notificationCorrelationId = new NotificationCorrelationId($"{orderApprovedEvent.Username}-{orderApprovedEvent.OrderId}");
            Username username = new Username(orderApprovedEvent.Username);
            NotificationTitle notificationTitle = new NotificationTitle($"Order Approved");
            NotificationContent notificationContent = new NotificationContent($"#{orderApprovedEvent.OrderId} order is approved at {orderApprovedEvent.OrderApprovedDate}");

            CreateNotificationCommand createNotificationCommand = new CreateNotificationCommand(notificationCorrelationId, username, notificationTitle, notificationContent, orderApprovedEvent.OrderApprovedDate);
            await SendCreateNotificationCommandAsync(createNotificationCommand);
        }

        public async Task Consume(ConsumeContext<OrderShippedEvent> context)
        {
            OrderShippedEvent orderShippedEvent = context.Message;

            NotificationCorrelationId notificationCorrelationId = new NotificationCorrelationId($"{orderShippedEvent.Username}-{orderShippedEvent.OrderId}-{orderShippedEvent.ShipmentId}");
            Username username = new Username(orderShippedEvent.Username);
            NotificationTitle notificationTitle = new NotificationTitle($"Order Shipped");
            NotificationContent notificationContent = new NotificationContent($"#{orderShippedEvent.OrderId} order is shipped at {orderShippedEvent.ShipmentDate}");

            CreateNotificationCommand createNotificationCommand = new CreateNotificationCommand(notificationCorrelationId, username, notificationTitle, notificationContent, orderShippedEvent.ShipmentDate);
            await SendCreateNotificationCommandAsync(createNotificationCommand);
        }

        private async Task SendCreateNotificationCommandAsync(CreateNotificationCommand createNotificationCommand)
        {
            try
            {
                await _notificationService.CreateNotificationAsync(createNotificationCommand, CancellationToken.None);
            }
            catch (NotificationAlreadyExistException)
            {
                // continue
            }
        }
    }
}