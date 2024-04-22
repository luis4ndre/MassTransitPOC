namespace Events
{
    public interface IReserveEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
    }
}
