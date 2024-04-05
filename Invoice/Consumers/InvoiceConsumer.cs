using Events;
using MassTransit;

namespace Invoice.Consumers
{
    public class InvoiceConsumer(ILogger<InvoiceConsumer> logger) : IConsumer<IInvoiceEvent>
    {
        private readonly ILogger<InvoiceConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<IInvoiceEvent> context)
        {
            var data = context.Message;

            var success = true;
            string? msg = null;

            if (data != null)
            {
                await context.Publish<INotificationEvent>(new
                {
                    data.OrderId,
                    data.Client,
                    data.CurrencyCode,
                    data.Amount,
                    Purchased = success,
                    Message = msg
                });

                _logger.LogInformation("Enviado para notificação!");
            }
            else
                _logger.LogInformation("Compra não realizada, Motivo: Dados não encontrado!");
        }
    }
}
