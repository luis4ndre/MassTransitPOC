using MassTransit;

namespace Events
{
    [EntityName("masstransit-poc--reserve-event")]
    public interface IReserveEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
    }
}
