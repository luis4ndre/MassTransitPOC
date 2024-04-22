using MassTransit;
using SagaService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    cfg.AddSagaStateMachine<ExchangeStateMachine, ExchangeStateData>().InMemoryRepository();
});

var app = builder.Build();

app.Run();
