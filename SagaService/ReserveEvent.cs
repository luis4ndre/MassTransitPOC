using Events;
using MassTransit;

namespace SagaService
{
    [EntityName("masstransit-poc--reserve-event")]
    public class ReserveEvent(ExchangeStateData exchangeStateData) : IReserveEvent
    {
        private readonly ExchangeStateData _exchangeStateData = exchangeStateData;

        public Guid OrderId => _exchangeStateData.OrderId;
        public string? Client => _exchangeStateData.Client;
        public decimal Amount => _exchangeStateData.Amount;
    }
}
