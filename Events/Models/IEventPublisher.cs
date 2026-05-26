namespace UsersAPI.Infrastructure.Repositories
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(string queueName, T message);
    }
}