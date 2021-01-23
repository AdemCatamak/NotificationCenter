namespace NotificationCenter.Model.Exceptions.Imp
{
    public class UsernameEmptyException : ValidationException
    {
        public UsernameEmptyException() : base("Username should not be empty")
        {
        }
    }
}