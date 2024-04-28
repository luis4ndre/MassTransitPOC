using MassTransit;

namespace Events
{
    [EntityName("queue-new-order-event")]
    public interface INewOrderEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
    }
}
