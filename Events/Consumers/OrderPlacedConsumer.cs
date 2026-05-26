using Events.Models;
using MassTransit;
using PaymentsAPI.Events.Publishers;

namespace PaymentsAPI.Events.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly SqsEventPublisher _eventPublisher;

        public OrderPlacedConsumer(SqsEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
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

            await _eventPublisher.PublishAsync(
                "payment-processed-event",
                paymentProcessed
            );

            await _eventPublisher.PublishAsync(
                "payment-processed-catalog",
                paymentProcessed
            );
        }
    }
}
