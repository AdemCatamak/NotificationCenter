using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NotificationCenter.Model.Entities;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.DataAccessLayer.DatabaseContext.Repositories
{
    public interface INotificationRepository : IRepository
    {
        Task AddAsync(Notification notification, CancellationToken cancellationToken);

        Task<Notification> GetNotificationAsync(Username username, NotificationCorrelationId notificationCorrelationId, CancellationToken cancellationToken);

        Task<IEnumerable<Notification>> GetNotificationAsync(Username username, bool? isSeen, int skip, int take, CancellationToken cancellationToken);
        Task UpdateAsync(Notification notification, CancellationToken cancellationToken);
    }
}