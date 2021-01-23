namespace NotificationCenter.Model.Exceptions.Imp
{
    public class NotificationTitleEmptyException : ValidationException
    {
        public NotificationTitleEmptyException() : base("Notification title should not be empty")
        {
        }
    }
}