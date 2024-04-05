namespace Events
{
    public interface INewOrderEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public string? CurrencyCode { get; }
        public decimal Amount { get; }
    }
}
