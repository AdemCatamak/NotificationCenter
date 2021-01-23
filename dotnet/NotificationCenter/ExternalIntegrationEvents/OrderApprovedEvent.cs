using System;

namespace NotificationCenter.ExternalIntegrationEvents
{
    public class OrderApprovedEvent
    {
        // There is no control over fields. This is not our concern right now
        public string Username { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public DateTime OrderApprovedDate { get; set; }
    }
}