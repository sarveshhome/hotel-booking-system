namespace Kafka.Publishers.Interfaces
{
    public interface IKafkaProducer
    {
        Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
        Task PublishAsync<T>(string topic, T message, string key, CancellationToken cancellationToken = default) where T : class;
    }
}