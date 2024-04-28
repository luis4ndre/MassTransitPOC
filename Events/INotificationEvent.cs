using MassTransit;

namespace Events
{
    [EntityName("queue-notification-event")]
    public interface INotificationEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
        public bool Purchased { get; }
        public string? Message { get; }
    }
}
