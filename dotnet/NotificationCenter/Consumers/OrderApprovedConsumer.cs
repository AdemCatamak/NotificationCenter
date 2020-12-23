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
    public class OrderApprovedConsumer : IConsumer<OrderApprovedEvent>
    {
        private readonly INotificationService _notificationService;

        public OrderApprovedConsumer(INotificationService notificationService)
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