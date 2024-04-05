using Events;
using MassTransit;

namespace PurchaseApi.Consumers
{
    public class NewOrderConsumer(ILogger<NewOrderConsumer> logger) : IConsumer<INewOrderEvent>
    {
        private readonly ILogger<NewOrderConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<INewOrderEvent> context)
        {
            Thread.Sleep(1500);

            var data = context.Message;
            if (data is not null)
            {
                await context.Publish<IProvisionEvent>(new
                {
                    data.OrderId,
                    data.Client,
                    data.CurrencyCode,
                    data.Amount
                });

                _logger.LogInformation("O pedido do cliente {client} no valor de {amount} foi recepcionado!", data.Client, data.Amount);
            }
        }
    }
}
