using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Business.Commands
{
    public class ChangeNotificationSeenStatusCommand
    {
        public UserId UserId { get; }
        public NotificationCorrelationId CorrelationId { get; }

        public bool Seen { get; private set; }

        public ChangeNotificationSeenStatusCommand(UserId userId, NotificationCorrelationId correlationId, bool seen)
        {
            UserId = userId;
            CorrelationId = correlationId;
            Seen = seen;
        }
    }
}