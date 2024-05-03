using MassTransit;

namespace Events
{
    [EntityName("masstransit-poc--new-order-event")]
    public interface INewOrderEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
    }
}
