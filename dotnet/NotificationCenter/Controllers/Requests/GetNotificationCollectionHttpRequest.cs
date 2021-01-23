namespace NotificationCenter.Controllers.Requests
{
    public class GetNotificationCollectionHttpRequest
    {
        public bool? IsSeen { get; set; } = null;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}