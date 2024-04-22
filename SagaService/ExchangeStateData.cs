using MassTransit;

namespace SagaService
{
    public class ExchangeStateData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }
        public Guid OrderId { get; set; }
        public string? Client { get; set; }
        public decimal Amount { get; set; }
        public decimal Limit { get; set; }
        public string? Message { get; internal set; }
        public bool Purchased { get; internal set; }
    }
}
