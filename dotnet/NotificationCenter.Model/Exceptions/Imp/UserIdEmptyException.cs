namespace NotificationCenter.Model.Exceptions.Imp
{
    public class UserIdEmptyException : ValidationException
    {
        public UserIdEmptyException() : base("User identifier should not be empty")
        {
        }
    }
}