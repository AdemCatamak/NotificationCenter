using System;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Business.Commands
{
    public class QueryNotificationCommand
    {
        public Username Username { get; }
        public bool? IsSeen { get; }
        public int Skip { get; }
        public int Take { get; }

        public QueryNotificationCommand(Username username, bool? isSeen, int skip, int take)
        {
            Username = username;
            IsSeen = isSeen;
            Skip = skip;
            Take = take;
        }
    }

    public class NotificationResponse
    {
        public string CorrelationId { get; }
        public string Title { get; }
        public string Content { get; }
        public bool IsSeen { get; }
        public DateTime OperationDate { get; }

        public NotificationResponse(string correlationId, string title, string content, bool isSeen, DateTime operationDate)
        {
            CorrelationId = correlationId;
            Title = title;
            Content = content;
            IsSeen = isSeen;
            OperationDate = operationDate;
        }
    }
}