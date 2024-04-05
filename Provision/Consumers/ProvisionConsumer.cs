using Events;
using MassTransit;

namespace Provision.Consumers
{
    public class ProvisionConsumer(ILogger<ProvisionConsumer> logger) : IConsumer<IProvisionEvent>
    {
        private static readonly IDictionary<string, decimal> _limits = new Dictionary<string, decimal>()
        {
            ["WESTERN"] = 1000,
            ["NOMAD"] = 2000
        };

        private readonly ILogger<ProvisionConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<IProvisionEvent> context)
        {
            var data = context.Message;

            if (data != null)
            {
                var clientLimit = _limits.SingleOrDefault(s => s.Key == data.Client?.ToUpper()).Value;
                if (clientLimit == 0)
                    clientLimit = 500M;

                if (clientLimit >= data.Amount)
                {
                    await context.Publish<IInvoiceEvent>(new
                    {
                        data.OrderId,
                        data.Client,
                        data.CurrencyCode,
                        data.Amount
                    });

                    _logger.LogInformation("Enviado para geração de boleto!");
                }
                else
                {
                    await context.Publish<ILoanEvent>(new
                    {
                        data.OrderId,
                        data.Client,
                        data.CurrencyCode,
                        data.Amount,
                        Limit = clientLimit
                    });

                    _logger.LogInformation("Enviado para analize de emprestimo!");
                }
            }
            else
                _logger.LogInformation("Compra não realizada, Motivo: Dados não encontrado!");
        }
    }
}
