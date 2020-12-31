using System;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Model.Entities
{
    public class Notification : ICustomEntity
    {
        public string Id => CorrelationId.Value;
        public NotificationCorrelationId CorrelationId { get; private set; } = null!;
        public Username Username { get; private set; } = null!;
        public NotificationTitle Title { get; private set; } = null!;
        public NotificationContent Content { get; private set; } = null!;
        public bool IsSeen { get; private set; }
        public DateTime? SeenOn { get; private set; }
        public DateTime OperationDate { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public Notification(NotificationCorrelationId correlationId, Username username, NotificationTitle title, NotificationContent content, DateTime operationDate)
            : this(correlationId, username, title, content, false, operationDate, DateTime.UtcNow)
        {
        }

        public Notification(NotificationCorrelationId correlationId, Username username, NotificationTitle title, NotificationContent content, bool isSeen, DateTime operationDate, DateTime createdOn)
        {
            CorrelationId = correlationId;
            Username = username;
            Title = title;
            Content = content;
            IsSeen = isSeen;
            OperationDate = operationDate;
            CreatedOn = createdOn;
        }

        public void ChangeSeenStatus(bool isSeen)
        {
            SeenOn = isSeen
                         ? DateTime.UtcNow
                         : null as DateTime?;
            IsSeen = isSeen;
        }
    }
}