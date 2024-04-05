using MassTransit;
using Provision.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("rabbitmq://localhost/"), hst =>
        {
            hst.Username("guest");
            hst.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });

    cfg.AddConsumer<ProvisionConsumer>(c => {
        c.UseMessageRetry(r =>
        {
            r.Ignore<ArgumentNullException>();
            r.Interval(3, TimeSpan.FromMilliseconds(1000));
        });
    });

    //cfg.AddConsumer<CancelSendingEmailConsumer>();
});

var app = builder.Build();

app.Run();
