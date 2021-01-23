using System;

namespace NotificationCenter.ExternalIntegrationEvents
{
    public class OrderShippedEvent
    {
        // There is no control over fields. This is not our concern right now
        public string Username { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string ShipmentId { get; set; } = string.Empty;
        public DateTime ShipmentDate { get; set; }
    }
}