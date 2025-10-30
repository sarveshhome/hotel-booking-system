namespace Kafka.ConsumerHost.Interfaces
{
    public interface IMessageHandler<T> where T : class
    {
        Task HandleAsync(T message, CancellationToken cancellationToken);
    }
}