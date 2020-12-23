using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NotificationCenter.DataAccessLayer.DatabaseContext.ConfigModels;
using NotificationCenter.DataAccessLayer.DatabaseContext.Repositories;
using NotificationCenter.Model.Entities;
using NotificationCenter.Model.Exceptions;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.DataAccessLayer.DatabaseContext.MongoDbSection.Repositories
{
    public class NotificationRepository : INotificationRepository,
                                          IMongoRepository
    {
        private IMongoCollection<Notification> DbSet { get; }

        public NotificationRepository(IMongoClient mongoClient, NoSqlDbOption noSqlDbOption)
        {
            DbSet = mongoClient.GetDatabase(noSqlDbOption.DbName).GetCollection<Notification>(nameof(Notification));
        }

        public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
        {
            await DbSet.InsertOneAsync(notification, options: null, cancellationToken);
        }

        public async Task<Notification> GetNotificationAsync(Username username, NotificationCorrelationId notificationCorrelationId, CancellationToken cancellationToken)
        {
            Notification? notification = await (await DbSet.FindAsync(x => x.Username.Value == username.Value && x.CorrelationId.Value == notificationCorrelationId.Value,
                                                                      new FindOptions<Notification>()
                                                                      {
                                                                          Skip = 0,
                                                                          Limit = 1,
                                                                      },
                                                                      cancellationToken)
                                               ).FirstOrDefaultAsync(cancellationToken);
            if (notification == null) throw new NotFoundException<Notification>();

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationAsync(Username username, bool? isSeen, int skip, int take, CancellationToken cancellationToken)
        {
            IMongoQueryable<Notification>? queryable = DbSet.AsQueryable();
            queryable = queryable.Where(x => x.Username.Value == username.Value);
            if (isSeen.HasValue)
                queryable = queryable.Where(x => x.IsSeen == isSeen.Value);

            queryable = queryable.OrderByDescending(x => x.CreatedOn);

            List<Notification> notificationList = await queryable.Skip(skip).Take(take).ToListAsync(cancellationToken);

            if (!notificationList.Any()) throw new NotFoundException<Notification>();

            return notificationList;
        }

        public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken)
        {
            await DbSet.ReplaceOneAsync(x => x.Username.Value == notification.Username.Value && x.CorrelationId.Value == notification.CorrelationId.Value,
                                        notification,
                                        new ReplaceOptions {IsUpsert = false},
                                        cancellationToken);
        }
    }
}