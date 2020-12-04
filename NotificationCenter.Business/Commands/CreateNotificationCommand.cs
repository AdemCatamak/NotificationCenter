using System;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Business.Commands
{
    public class CreateNotificationCommand
    {
        public NotificationCorrelationId NotificationCorrelationId { get; private set; }
        public UserId UserId { get; private set; }
        public NotificationTitle NotificationTitle { get; private set; }
        public NotificationContent NotificationContent { get; private set; }
        public DateTime OperationDate { get; private set; }

        public CreateNotificationCommand(NotificationCorrelationId notificationCorrelationId, UserId userId, NotificationTitle notificationTitle, NotificationContent notificationContent, DateTime operationDate)
        {
            NotificationCorrelationId = notificationCorrelationId;
            UserId = userId;
            NotificationTitle = notificationTitle;
            NotificationContent = notificationContent;
            OperationDate = operationDate;
        }
    }
}