using System;

namespace NotificationCenter.Model.Exceptions
{
    public abstract class ConflictException : Exception
    {
        protected ConflictException(string message) : base(message)
        {
        }
    }
}