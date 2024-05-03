using Events;
using MassTransit;
using PurchaseApi.Consumers;
using PurchaseApi.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

    x.AddConsumer<NotificationConsumer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("/purchase", async (OrderDTO request, IBus bus) =>
{
    await bus.Publish<INewOrderEvent>(new
    {
        OrderId = Guid.NewGuid(),
        request.Client,
        request.Amount,
        OrderCreatedDate = DateTime.Now
    });

    return Results.Created();
});

app.Run();
