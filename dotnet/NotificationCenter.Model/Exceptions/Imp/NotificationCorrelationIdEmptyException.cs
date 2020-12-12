namespace NotificationCenter.Model.Exceptions.Imp
{
    public class NotificationCorrelationIdEmptyException : ValidationException
    {
        public NotificationCorrelationIdEmptyException() : base("Notification correlation identifier should not be empty")
        {
        }
    }
}