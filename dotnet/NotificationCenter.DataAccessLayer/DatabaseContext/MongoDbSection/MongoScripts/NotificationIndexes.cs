using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using NotificationCenter.DataAccessLayer.DatabaseContext.ConfigModels;
using NotificationCenter.Model.Entities;

namespace NotificationCenter.DataAccessLayer.DatabaseContext.MongoDbSection.MongoScripts
{
    public class NotificationIndexes : IMongoDbScript
    {
        private readonly IMongoClient _mongoClient;
        private readonly NoSqlDbOption _noSqlDbOption;

        public NotificationIndexes(IMongoClient mongoClient, NoSqlDbOption noSqlDbOption)
        {
            _mongoClient = mongoClient;
            _noSqlDbOption = noSqlDbOption;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            IMongoDatabase? database = _mongoClient.GetDatabase(_noSqlDbOption.DbName);
            IMongoCollection<Notification> notifications = database.GetCollection<Notification>(nameof(Notification));

            var options = new CreateIndexOptions {Unique = true};
            IndexKeysDefinition<Notification> correlationIdIndexDefinition = $"{{ {nameof(Notification.Username)}: 1, {nameof(Notification.CorrelationId)}: 1}}";
            var correlationIdIndex = new CreateIndexModel<Notification>(correlationIdIndexDefinition, options);

            await notifications.Indexes.CreateOneAsync(correlationIdIndex, cancellationToken: cancellationToken);
        }
    }
}