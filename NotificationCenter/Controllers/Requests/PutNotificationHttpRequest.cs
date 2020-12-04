namespace NotificationCenter.Controllers.Requests
{
    public class PutNotificationHttpRequest
    {
        public string? CorrelationId { get; set; }
        public string? Title { get; set; } 
        public string? Content { get; set; }
    }
}