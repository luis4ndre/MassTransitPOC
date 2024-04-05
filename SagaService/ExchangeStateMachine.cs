using Events;
using MassTransit;

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

        public ExchangeStateMachine()
        {
            InstanceState(s => s.CurrentState);

            Event(() => NewOrderEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => InvoiceEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => NewOrderEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => LoanEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => NotificationEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => ProvisionEvent, a => a.CorrelateById(m => m.Message.OrderId));

            Initially(
                When(NewOrderEvent).Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.CurrencyCode = context.Message.CurrencyCode;
                    context.Saga.Amount = context.Message.Amount;
                }).TransitionTo(NewOrder).Publish(context => new NewOrderEvent(context.Saga)));

            During(NewOrder,
                When(ProvisionEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.CurrencyCode = context.Message.CurrencyCode;
                    context.Saga.Amount = context.Message.Amount;
                }).TransitionTo(Provision));

            During(Provision,
                When(InvoiceEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.CurrencyCode = context.Message.CurrencyCode;
                    context.Saga.Amount = context.Message.Amount;
                }).TransitionTo(Invoice),
                When(LoanEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.CurrencyCode = context.Message.CurrencyCode;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Limit = context.Message.Limit;
                }).TransitionTo(Loan));

            During(Invoice,
                When(NotificationEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.CurrencyCode = context.Message.CurrencyCode;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Purchased = context.Message.Purchased;
                    context.Saga.Message = context.Message.Message;
                }).TransitionTo(Notification));

            During(Loan,
                When(NotificationEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.CurrencyCode = context.Message.CurrencyCode;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Purchased = context.Message.Purchased;
                    context.Saga.Message = context.Message.Message;
                }).TransitionTo(Notification),
                When(InvoiceEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.CurrencyCode = context.Message.CurrencyCode;
                    context.Saga.Amount = context.Message.Amount;
                }).TransitionTo(Invoice));
        }
    }
}
