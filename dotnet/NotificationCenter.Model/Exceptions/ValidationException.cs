using System;

namespace NotificationCenter.Model.Exceptions
{
    public abstract class ValidationException : Exception
    {
        protected ValidationException(string message) : base(message)
        {
        }
    }
}