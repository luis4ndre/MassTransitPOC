using Loan.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    cfg.AddConsumer<LoanConsumer>(c => {
        c.UseMessageRetry(r =>
        {
            r.Ignore<ArgumentNullException>();
            r.Interval(3, TimeSpan.FromMilliseconds(1000));
        });
    });
});

var app = builder.Build();

app.Run();
