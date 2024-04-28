using MassTransit;

namespace Events
{
    [EntityName("queue-invoice-event")]
    public interface IInvoiceEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
        public bool Loan { get;}
    }
}
