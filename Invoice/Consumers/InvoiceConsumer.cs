using Events;
using MassTransit;

namespace Invoice.Consumers
{
    public class InvoiceConsumer(ILogger<InvoiceConsumer> logger) : IConsumer<IInvoiceEvent>
    {
        private readonly ILogger<InvoiceConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<IInvoiceEvent> context)
        {
            Thread.Sleep(1500);

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

                _logger.LogInformation("O pedido do cliente {client} no valor de {amount} foi enviado para notificação!", data.Client, data.Amount);
            }
            else
                _logger.LogInformation("Compra não realizada, Motivo: Dados não encontrado!");
        }
    }
}
