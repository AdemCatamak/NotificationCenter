using System;
using System.Collections.Generic;
using System.Linq;
using NotificationCenter.DataAccessLayer.DatabaseContext.Exceptions;

namespace NotificationCenter.DataAccessLayer.DatabaseContext.ConfigModels
{
    public class NoSqlDbConfig
    {
        public int SelectedIndex { get; set; }
        public List<NoSqlDbOption> NoSqlDbOptions { get; set; } = new List<NoSqlDbOption>();

        public NoSqlDbOption SelectedDbOption()
        {
            if (NoSqlDbOptions == null)
                throw new ArgumentNullException(nameof(NoSqlDbOptions));

            if (!NoSqlDbOptions.Any())
                throw new ArgumentException($"{nameof(NoSqlDbOptions)} is empty");

            var noSqlDbOption = NoSqlDbOptions.FirstOrDefault(o => o.Index == SelectedIndex);

            if (noSqlDbOption == null)
                throw new DbOptionNotFoundException(SelectedIndex);

            return noSqlDbOption;
        }
    }

    public class NoSqlDbOption
    {
        public int Index { get; set; }
        public NoSqlDbTypes NoSqlDbType { get; set; }
        public string DbName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<NoSqlNode> Nodes { get; set; } = new List<NoSqlNode>();
    }

    public class NoSqlNode
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }

    public enum NoSqlDbTypes
    {
        MongoDb = 1
    }
}