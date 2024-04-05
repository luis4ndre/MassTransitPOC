namespace Events
{
    public interface IProvisionEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public string? CurrencyCode { get; }
        public decimal Amount { get; }
    }
}
