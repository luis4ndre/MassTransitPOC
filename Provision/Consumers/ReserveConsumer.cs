using Events;
using MassTransit;
using Provision.Exceptions;

namespace Provision.Consumers
{
    public class ReserveConsumer(ILogger<ReserveConsumer> logger) : IConsumer<IReserveEvent>
    {
        private static readonly IDictionary<string, decimal> _limits = new Dictionary<string, decimal>()
        {
            ["WESTERN"] = 1000,
            ["NOMAD"] = 2000
        };

        private readonly ILogger<ReserveConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<IReserveEvent> context)
        {
            _logger.LogInformation("IReserveEvent Start");

            Thread.Sleep(1500);

            var data = context.Message;

            if (data != null)
            {
                if (data.Client?.ToUpper() == "RETRY" && context.GetRetryAttempt() < 3)
                {
                    _logger.LogWarning("Tentativa {try}", context.GetRetryAttempt() + 1);
                    throw new RetryException("Testando a retentativa!");
                }

                if (data.Client?.ToUpper() == "ERROR")
                {
                    throw new IgnoreException("Testando erro não 'retentavel'!");
                }

                var clientLimit = _limits.SingleOrDefault(s => s.Key == data.Client?.ToUpper()).Value;
                if (clientLimit == 0)
                    clientLimit = 500M;

                if (clientLimit >= data.Amount)
                {
                    await context.Publish<IInvoiceEvent>(new
                    {
                        data.OrderId,
                        data.Client,
                        data.Amount
                    });

                    _logger.LogInformation("O pedido do cliente {client} no valor de {amount} foi enviado para geração de boleto!", data.Client, data.Amount);
                }
                else
                {
                    await context.Publish<ILoanEvent>(new
                    {
                        data.OrderId,
                        data.Client,
                        data.Amount,
                        Limit = clientLimit
                    });

                    _logger.LogInformation("O pedido do cliente {client} no valor de {amount} foi enviado para analise de emprestimo!", data.Client, data.Amount);
                }
            }
            else
                _logger.LogInformation("Compra não realizada, Motivo: Dados não encontrado!");
        }
    }
}
