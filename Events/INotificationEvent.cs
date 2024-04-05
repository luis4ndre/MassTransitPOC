namespace Events
{
    public interface INotificationEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public string? CurrencyCode { get; }
        public decimal Amount { get; }
        public bool Purchased { get; }
        public string? Message { get; }
    }
}
