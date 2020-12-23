using System;

namespace NotificationCenter.Business.Events
{
    public class NotificationSeenStatusChangedEvent
    {
        public string CorrelationId { get; private set; } = null!;
        public string Username { get; private set; } = null!;
        public string Title { get; private set; } = null!;
        public string Content { get; private set; } = null!;
        public DateTime OperationDate { get; private set; }
        public bool IsSeen { get; private set; }

        public NotificationSeenStatusChangedEvent(string correlationId, string username, string title, string content, DateTime operationDate, bool isSeen)
        {
            CorrelationId = correlationId;
            Username = username;
            Title = title;
            Content = content;
            OperationDate = operationDate;
            IsSeen = isSeen;
        }
    }
}