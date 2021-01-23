using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NotificationCenter.Business.Commands;

namespace NotificationCenter.Business.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(CreateNotificationCommand createNotificationCommand, CancellationToken cancellationToken);
        Task<List<NotificationResponse>> QueryNotificationAsync(QueryNotificationCommand queryNotificationCommand, CancellationToken cancellationToken);
        Task ChangeNotificationSeenStatusAsync(ChangeNotificationSeenStatusCommand changeNotificationSeenStatusCommand, CancellationToken none);
    }
}