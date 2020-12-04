using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Model.Exceptions.Imp
{
    public class NotificationAlreadyExistException : ConflictException
    {
        public NotificationAlreadyExistException(NotificationCorrelationId notificationCorrelationId) : base($"#{notificationCorrelationId} notification is already registered")
        {
        }
    }
}