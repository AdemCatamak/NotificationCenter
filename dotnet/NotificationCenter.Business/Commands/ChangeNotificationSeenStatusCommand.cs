using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Business.Commands
{
    public class ChangeNotificationSeenStatusCommand
    {
        public Username Username { get; }
        public NotificationCorrelationId CorrelationId { get; }

        public bool Seen { get; private set; }

        public ChangeNotificationSeenStatusCommand(Username username, NotificationCorrelationId correlationId, bool seen)
        {
            Username = username;
            CorrelationId = correlationId;
            Seen = seen;
        }
    }
}