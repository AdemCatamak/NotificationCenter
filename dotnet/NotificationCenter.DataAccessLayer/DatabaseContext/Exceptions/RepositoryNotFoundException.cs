using System;

namespace NotificationCenter.DataAccessLayer.DatabaseContext.Exceptions
{
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException(Type t) : base($"{t.FullName} could not found")
        {
            
        }
    }
}