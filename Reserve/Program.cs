using MassTransit;
using Reserve.Consumers;
using Reserve.Exceptions;

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

    x.AddConsumer<ReserveConsumer>(c => {
        c.UseMessageRetry(r =>
        {
            r.Ignore<IgnoreException>();
            r.Interval(5, TimeSpan.FromMilliseconds(1000));
        });
    });
});

var app = builder.Build();

app.Run();
