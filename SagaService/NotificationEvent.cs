using Events;

namespace SagaService
{
    public class NotificationEvent(ExchangeStateData exchangeStateData) : INotificationEvent
    {
        private readonly ExchangeStateData _exchangeStateData = exchangeStateData;

        public Guid OrderId => _exchangeStateData.OrderId;
        public string? Client => _exchangeStateData.Client;
        public decimal Amount => _exchangeStateData.Amount;
        public bool Purchased => _exchangeStateData.Purchased;
        public string? Message => _exchangeStateData.Message;
    }
}
