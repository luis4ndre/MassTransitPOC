using Events;
using MassTransit;

namespace PurchaseApi.Consumers
{
    public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<INotificationEvent>
    {
        private readonly ILogger<NotificationConsumer> _logger = logger;

        public Task Consume(ConsumeContext<INotificationEvent> context)
        {
            _logger.LogInformation("INotificationEvent Start");

            Thread.Sleep(1500);

            var data = context.Message;

            if (data != null && data.Purchased)
                _logger.LogInformation("Compra do cliente {client} no valor de {amount} realizada com Sucesso", data.Client, data.Amount);
            else if (data != null)
                _logger.LogInformation("Compra do cliente {client} no valor de {amount} não realizada, Motivo: {msg}", data.Client, data.Amount, data.Message);
            else
                _logger.LogInformation("Compra não realizada, Motivo: Dados não encontrado!");

            return Task.CompletedTask;
        }
    }
}
