using MassTransit;
using Events.Models;

namespace PaymentsAPI.Events.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderPlacedConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"Processing payment for User {message.UserId}");

            var approved = new Random().Next(0, 2) == 1;

            var paymentProcessed = new PaymentProcessedEvent
            {
                GameId = message.GameId,
                UserId = message.UserId,
                Price = message.Price,
                Status = approved ? "Approved" : "Rejected"
            };

            await _publishEndpoint.Publish(paymentProcessed);
        }
    }
}
