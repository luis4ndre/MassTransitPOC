using MassTransit;
using SagaService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "masstransit-poc--", includeNamespace: false));

    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("sa-east-1", h =>
        {
            h.AccessKey("your-iam-access-key");
            h.SecretKey("your-iam-secret-key");
        });

        cfg.ConfigureEndpoints(context);
    });

    x.AddSagaStateMachine<ExchangeStateMachine, ExchangeStateData>().InMemoryRepository();
});

var app = builder.Build();

app.Run();
