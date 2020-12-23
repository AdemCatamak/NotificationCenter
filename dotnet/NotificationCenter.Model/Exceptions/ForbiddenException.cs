using System;

namespace NotificationCenter.Model.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base(string.Empty)
        {
        }
    }
}