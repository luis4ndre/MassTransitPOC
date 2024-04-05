using Events;
using MassTransit;

namespace PurchaseApi.Consumers
{
    public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<INotificationEvent>
    {
        private readonly ILogger<NotificationConsumer> _logger = logger;

        public Task Consume(ConsumeContext<INotificationEvent> context)
        {
            var data = context.Message;

            if (data != null && data.Purchased)
                _logger.LogInformation("Compra realizada com Sucesso");
            else
                _logger.LogInformation("Compra não realizada, Motivo: {msg}", data == null ? "Dados não encontrado!" : data.Message);

            return Task.CompletedTask;
        }
    }
}
