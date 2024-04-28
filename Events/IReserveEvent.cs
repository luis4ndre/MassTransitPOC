using MassTransit;

namespace Events
{
    [EntityName("queue-reserve-event")]
    public interface IReserveEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
    }
}
