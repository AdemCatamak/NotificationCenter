using System;

namespace NotificationCenter.DataAccessLayer.DatabaseContext.Exceptions
{
    public class DbOptionNotFoundException : Exception
    {
        public DbOptionNotFoundException(int selectedIndex) : base($"NoSqlDbOption could not found. [SelectedIndex : {selectedIndex}]")
        {
        }
    }
}