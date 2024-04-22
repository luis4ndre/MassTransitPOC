using Events;
using MassTransit;

namespace Loan.Consumers
{
    public class LoanConsumer(ILogger<LoanConsumer> logger) : IConsumer<ILoanEvent>
    {
        private static readonly IDictionary<string, decimal> _percents = new Dictionary<string, decimal>()
        {
            ["WESTERN"] = 50,
            ["NOMAD"] = 30
        };

        private readonly ILogger<LoanConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<ILoanEvent> context)
        {
            _logger.LogInformation("ILoanEvent Start");

            Thread.Sleep(1500);

            var data = context.Message;

            if (data != null)
            {
                var clientPercents = _percents.SingleOrDefault(s => s.Key == data.Client?.ToUpper()).Value + 100;
                if (clientPercents == 100)
                    clientPercents = 105;

                if ((data.Amount / data.Limit * 100) <= clientPercents)
                {
                    await context.Publish<IInvoiceEvent>(new
                    {
                        data.OrderId,
                        data.Client,
                        data.Amount,
                        Loan = true
                    });

                    _logger.LogInformation("Emprestimo ao cliente {client} no valor de {amount} foi concedido!", data.Client, data.Amount);
                    _logger.LogInformation("Enviado para geração de boleto!");
                }
                else
                {
                    await context.Publish<INotificationEvent>(new
                    {
                        data.OrderId,
                        data.Client,
                        data.Amount,
                        Purchased = false,
                        Message = "Empretimo não concedido!"
                    });

                    _logger.LogInformation("Emprestimo ao cliente {client} no valor de {amount} foi negado, valor superior a {percent}%!", data.Client, data.Amount, clientPercents);
                    _logger.LogInformation("Enviado para notificação!");
                }
            }
            else
                _logger.LogInformation("Compra não realizada, Motivo: Dados não encontrado!");
        }
    }
}
