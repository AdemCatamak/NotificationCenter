using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NotificationCenter.Business.Commands;
using NotificationCenter.Business.Services;
using NotificationCenter.Consumers.IntegrationEvents;
using NotificationCenter.Model.Exceptions.Imp;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Consumers
{
    public class OrderShippedConsumer : IConsumer<OrderShippedEvent>
    {
        private readonly INotificationService _notificationService;

        public OrderShippedConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<OrderShippedEvent> context)
        {
            OrderShippedEvent orderShippedEvent = context.Message;

            NotificationCorrelationId notificationCorrelationId = new NotificationCorrelationId($"{orderShippedEvent.UserId}-{orderShippedEvent.OrderId}-{orderShippedEvent.ShipmentId}");
            UserId userId = new UserId(orderShippedEvent.UserId);
            NotificationTitle notificationTitle = new NotificationTitle($"Order Shipped");
            NotificationContent notificationContent = new NotificationContent($"#{orderShippedEvent.OrderId} order is shipped at {orderShippedEvent.ShipmentDate}");

            CreateNotificationCommand createNotificationCommand = new CreateNotificationCommand(notificationCorrelationId, userId, notificationTitle, notificationContent, orderShippedEvent.ShipmentDate);
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