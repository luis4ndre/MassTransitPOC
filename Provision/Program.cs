using MassTransit;
using Provision.Consumers;
using Provision.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    cfg.AddConsumer<ReserveConsumer>(c => {
        c.UseMessageRetry(r =>
        {
            //r.Ignore<IgnoreException>();
            r.Interval(5, TimeSpan.FromMilliseconds(1000));
        });
    });
});

var app = builder.Build();

app.Run();
