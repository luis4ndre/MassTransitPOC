using Events;
using MassTransit;
using PurchaseApi.Consumers;
using PurchaseApi.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("rabbitmq://localhost/"), hst =>
        {
            hst.Username("guest");
            hst.Password("guest");
        });

        cfg.ReceiveEndpoint("Saga-Queue", ep =>
        {
            ep.PrefetchCount = 10;
            // Get Consumer
            ep.ConfigureConsumer<NewOrderConsumer>(context);
            // Cancel Consumer
            ep.ConfigureConsumer<NotificationConsumer>(context);
        });

        //cfg.ConfigureEndpoints(context);
    });

    cfg.AddConsumer<NewOrderConsumer>();
    cfg.AddConsumer<NotificationConsumer>();
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
    var endPoint = await bus.GetSendEndpoint(new Uri("queue:Saga-Queue"));
    await endPoint.Send<INewOrderEvent>(new
    {
        OrderId = Guid.NewGuid(),
        request.Client,
        request.CurrencyCode,
        request.Amount,
        OrderCreatedDate = DateTime.Now
    });

    return Results.Created();
});

app.Run();
