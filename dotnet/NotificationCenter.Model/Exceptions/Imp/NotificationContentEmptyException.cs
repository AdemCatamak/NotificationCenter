namespace NotificationCenter.Model.Exceptions.Imp
{
    public class NotificationContentEmptyException : ValidationException
    {
        public NotificationContentEmptyException() : base("Notification content should not be empty")
        {
        }
    }
}