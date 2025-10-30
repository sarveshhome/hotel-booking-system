using Confluent.Kafka;
using Kafka.Publishers.Configuration;
using Kafka.Publishers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Kafka.Publishers
{
    public class KafkaProducer : IKafkaProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IOptions<KafkaSettings> settings, ILogger<KafkaProducer> logger)
        {
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = settings.Value.BootstrapServers,
                ClientId = settings.Value.ClientId,
                Acks = Acks.All,
                EnableIdempotence = true,
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
        {
            await PublishAsync(topic, message, Guid.NewGuid().ToString(), cancellationToken);
        }

        public async Task PublishAsync<T>(string topic, T message, string key, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                var value = JsonSerializer.Serialize(message);
                var kafkaMessage = new Message<string, string>
                {
                    Key = key,
                    Value = value
                };

                var deliveryResult = await _producer.ProduceAsync(
                    topic,
                    kafkaMessage,
                    cancellationToken
                );

                _logger.LogInformation(
                    "Message delivered: Topic: {Topic}, Partition: {Partition}, Offset: {Offset}",
                    deliveryResult.Topic, deliveryResult.Partition, deliveryResult.Offset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to topic {Topic}", topic);
                throw;
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}