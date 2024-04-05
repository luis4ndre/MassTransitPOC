using Events;

namespace SagaService
{
    public class NewOrderEvent(ExchangeStateData exchangeStateData) : INewOrderEvent
    {
        private readonly ExchangeStateData _exchangeStateData = exchangeStateData;

        public Guid OrderId => _exchangeStateData.OrderId;
        public string? Client => _exchangeStateData.Client;
        public string? CurrencyCode => _exchangeStateData.CurrencyCode;
        public decimal Amount => _exchangeStateData.Amount;
    }
}
