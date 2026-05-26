using MassTransit;
using PaymentsAPI.Events.Consumers;
using PaymentsAPI.Events.Publishers;
using PaymentsAPI.Middlewares;
using Amazon.SQS;

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
// SQS Config
// ======================

var AWS_SERVICE_URL = Environment.GetEnvironmentVariable("AWS_SERVICE_URL");
var AWS_USER = Environment.GetEnvironmentVariable("AWS_USER");
var AWS_PASSWORD = Environment.GetEnvironmentVariable("AWS_PASSWORD");

builder.Services.AddSingleton<IAmazonSQS>(_ =>
{
    return new AmazonSQSClient(
        AWS_USER,
        AWS_PASSWORD,
        new AmazonSQSConfig
        {
            ServiceURL = AWS_SERVICE_URL
        }
    );
});

builder.Services.AddSingleton<SqsEventPublisher>();

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