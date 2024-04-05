using Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace SagaService
{
    public class ExchangeStateMachine : MassTransitStateMachine<ExchangeStateData>
    {
        public State? NewOrder { get; private set; }
        public State? Provision { get; private set; }
        public State? Invoice { get; private set; }
        public State? Loan { get; private set; }
        public State? Notification { get; private set; }

        public Event<INewOrderEvent>? NewOrderEvent { get; private set; }
        public Event<IInvoiceEvent>? InvoiceEvent { get; private set; }
        public Event<ILoanEvent>? LoanEvent { get; private set; }
        public Event<INotificationEvent>? NotificationEvent { get; private set; }
        public Event<IProvisionEvent>? ProvisionEvent { get; private set; }

        public Event<Fault<IProvisionEvent>> ProvisionFaulted { get; private set; }

        public ExchangeStateMachine()
        {
            InstanceState(s => s.CurrentState);

            Event(() => NewOrderEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => InvoiceEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => LoanEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => NotificationEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => ProvisionEvent, a => a.CorrelateById(m => m.Message.OrderId));

            Event(() => ProvisionFaulted, a => a.CorrelateById(m => m.Message.Message.OrderId));

            Initially(
                When(NewOrderEvent).TransitionTo(NewOrder).Publish(context => new NewOrderEvent(context.Saga)));

            During(NewOrder,
                When(ProvisionEvent)
                .Then(context=> Console.WriteLine("TESTETESTETESTETESTETESTETESTETESTETESTE"))
                .TransitionTo(Provision)
                .Catch<Exception>(x =>
                     x.Then(context =>
                     {
                         context.Saga.OrderId = context.Message.OrderId;
                         context.Saga.Client = context.Message.Client;
                         context.Saga.CurrencyCode = context.Message.CurrencyCode;
                         context.Saga.Amount = context.Message.Amount;
                         context.Saga.Purchased = false;
                         context.Saga.Message = "Exception!! Exception!! Exception!! Exception!! Exception!! Exception!!";
                     }).TransitionTo(Notification)
                 ),
                When(ProvisionFaulted)
                .Then(context => Console.WriteLine("Exception!! Exception!! Exception!! Exception!! Exception!! Exception!!"))
                .TransitionTo(Notification)
                );

            During(Provision,
                When(InvoiceEvent).TransitionTo(Invoice),
                When(LoanEvent).TransitionTo(Loan));

            During(Invoice,
                When(NotificationEvent).TransitionTo(Notification));

            During(Loan,
                When(NotificationEvent).TransitionTo(Notification),
                When(InvoiceEvent).TransitionTo(Invoice));

            DuringAny(When(ProvisionFaulted)
            .Then(context =>
            {
                context.Saga.OrderId = context.Message.Message.OrderId;
                context.Saga.Client = context.Message.Message.Client;
                context.Saga.CurrencyCode = context.Message.Message.CurrencyCode;
                context.Saga.Amount = context.Message.Message.Amount;
                context.Saga.Purchased = false;
                context.Saga.Message = "Exception!! Exception!! Exception!! Exception!! Exception!! Exception!!";
            })
            .TransitionTo(Notification));
        }
    }
}
