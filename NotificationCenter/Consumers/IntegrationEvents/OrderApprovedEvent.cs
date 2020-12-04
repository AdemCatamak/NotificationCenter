using System;

namespace NotificationCenter.Consumers.IntegrationEvents
{
    public class OrderApprovedEvent
    {
        // There is no control over fields. This is not our concern right now
        public string UserId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public DateTime OrderApprovedDate { get; set; }
    }
}