using MassTransit;
using PaymentsAPI.Middlewares;
using PaymentsAPI.Events.Consumers;

var builder = WebApplication.CreateBuilder(args);

// ======================
// Controllers + Swagger
// ======================
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Payments API",
        Version = "v1"
    });
});

// ======================
// MassTransit Config
// ======================
builder.Services.AddMassTransit(x =>
{
    var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
    var username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
    var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
    var paymentQueue = Environment.GetEnvironmentVariable("PAYMENT_QUEUE");

    if (host == null || username == null || password == null || paymentQueue == null)
    {
        throw new Exception("RabbitMQ configuration is missing. Please set environment variables");
    }
    else
    {
        x.AddConsumer<OrderPlacedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(host, "/", h =>
            {
                h.Username(username);
                h.Password(password);
            });

            cfg.ReceiveEndpoint(paymentQueue, e =>
            {
                e.ConfigureConsumer<OrderPlacedConsumer>(context);
            });
        });
    }
});

// ======================
var app = builder.Build();
// ======================

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();