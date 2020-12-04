using System.Threading;
using System.Threading.Tasks;

namespace NotificationCenter.DataAccessLayer.DatabaseContext.MongoDbSection
{
    public interface IMongoDbScript
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}