using Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace SagaService
{
    public class ExchangeStateMachine : MassTransitStateMachine<ExchangeStateData>
    {
        public State? Received { get; private set; }
        public State? Invoice { get; private set; }
        public State? Loan { get; private set; }
        public State? Notification { get; private set; }

        public Event<INewOrderEvent>? NewOrderEvent { get; private set; }
        public Event<IInvoiceEvent>? InvoiceEvent { get; private set; }
        public Event<ILoanEvent>? LoanEvent { get; private set; }
        public Event<INotificationEvent>? NotificationEvent { get; private set; }

        public Event<Fault<IReserveEvent>>? ReserveFaulted { get; private set; }

        public ExchangeStateMachine()
        {
            InstanceState(s => s.CurrentState);

            Event(() => NewOrderEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => InvoiceEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => LoanEvent, a => a.CorrelateById(m => m.Message.OrderId));
            Event(() => NotificationEvent, a => a.CorrelateById(m => m.Message.OrderId));

            Event(() => ReserveFaulted, a => a.CorrelateById(m => m.Message.Message.OrderId));

            Initially(
                When(NewOrderEvent).Then(context =>
                {
                    Console.WriteLine("Initially => NewOrderEvent => NewOrder");

                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.Amount = context.Message.Amount;
                }).TransitionTo(Received).Publish(context => new ReserveEvent(context.Saga)));

            During(Received,
                When(InvoiceEvent)
                .Then(context =>
                {
                    Console.WriteLine("Reserve => InvoiceEvent => Invoice");

                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.Amount = context.Message.Amount;
                }).TransitionTo(Invoice),
                When(LoanEvent)
                .Then(context =>
                {
                    Console.WriteLine("Reserve => LoanEvent => Loan");

                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Limit = context.Message.Limit;
                }).TransitionTo(Loan));

            During(Invoice,
                When(NotificationEvent)
                .Then(context =>
                {
                    Console.WriteLine("Invoice => NotificationEvent => Notification");

                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Purchased = context.Message.Purchased;
                    context.Saga.Message = context.Message.Message;
                }).TransitionTo(Notification));

            During(Loan,
                When(NotificationEvent)
                .Then(context =>
                {
                    Console.WriteLine("Loan => NotificationEvent => Notification");

                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.Purchased = context.Message.Purchased;
                    context.Saga.Message = context.Message.Message;
                }).TransitionTo(Notification),
                When(InvoiceEvent)
                .Then(context =>
                {
                    Console.WriteLine("Loan => InvoiceEvent => Invoice");

                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Client = context.Message.Client;
                    context.Saga.Amount = context.Message.Amount;
                }).TransitionTo(Invoice));

            DuringAny(
                When(ReserveFaulted)
                .Then(context =>
                {
                    Console.WriteLine("Any => ReserveFaulted => Notification");

                    context.Saga.OrderId = context.Message.Message.OrderId;
                    context.Saga.Client = context.Message.Message.Client;
                    context.Saga.Amount = context.Message.Message.Amount;
                    context.Saga.Purchased = false;
                    context.Saga.Message = "ReserveFaulted";
                }).TransitionTo(Notification).Publish(context => new NotificationEvent(context.Saga)));

            During(Notification,
                When(NotificationEvent)
                .Then(context =>
                {
                    Console.WriteLine("Finalize");
                })
                .Finalize());
        }
    }
}
