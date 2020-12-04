using System;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Business.Commands
{
    public class QueryNotificationCommand
    {
        public UserId UserId { get; }
        public bool? IsSeen { get; }
        public int Skip { get; }
        public int Take { get; }

        public QueryNotificationCommand(UserId userId, bool? isSeen, int skip, int take)
        {
            UserId = userId;
            IsSeen = isSeen;
            Skip = skip;
            Take = take;
        }
    }

    public class NotificationResponse
    {
        public NotificationCorrelationId CorrelationId { get; }
        public NotificationTitle Title { get; }
        public NotificationContent Content { get; }
        public bool IsSeen { get; }
        public DateTime OperationDate { get; }

        public NotificationResponse(NotificationCorrelationId correlationId, NotificationTitle title, NotificationContent content, bool isSeen, DateTime operationDate)
        {
            CorrelationId = correlationId;
            Title = title;
            Content = content;
            IsSeen = isSeen;
            OperationDate = operationDate;
        }
    }
}