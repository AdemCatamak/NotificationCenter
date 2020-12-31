using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotificationCenter.Business.Commands;

namespace NotificationCenter.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public const string GET_NOTIFICATION = "GetNotification";

        [HubMethodName("ChangeNotificationSeenStatus")]
        public async Task ChangeNotificationSeenStatusAsync(string notificationCorrelationId)
        {
            // TODO kullanıcının diğer makinalarda açık olan bağlantılarına değişiklik bilgisi iletilmeli
            var username = this.Context.UserIdentifier;
            var username2 = this.Context.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            await this.Clients.Users(username2)
                      .SendAsync("x", notificationCorrelationId);
        }
    }
}