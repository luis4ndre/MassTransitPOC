namespace Events
{
    public interface IInvoiceEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public string? CurrencyCode { get; }
        public decimal Amount { get; }
        public bool Loan { get;}
    }
}
