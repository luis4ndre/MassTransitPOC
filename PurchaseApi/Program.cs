using Events;
using MassTransit;
using PurchaseApi.Consumers;
using PurchaseApi.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

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
