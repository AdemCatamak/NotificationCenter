using System;

namespace NotificationCenter.Consumers.IntegrationEvents
{
    public class OrderShippedEvent
    {
        // There is no control over fields. This is not our concern right now
        public string UserId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string ShipmentId { get; set; } = string.Empty;
        public DateTime ShipmentDate { get; set; }
    }
}