// src/Kafka/ConsumerHost/KafkaConsumerService.cs
using Confluent.Kafka;
using Kafka.ConsumerHost.Configuration;
using Kafka.ConsumerHost.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Kafka.ConsumerHost
{
    public class KafkaConsumerService<T> : BackgroundService where T : class
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IMessageHandler<T> _messageHandler;
        private readonly ILogger<KafkaConsumerService<T>> _logger;
        private readonly string _topic;

        public KafkaConsumerService(
            IOptions<KafkaConsumerSettings> settings,
            IMessageHandler<T> messageHandler,
            ILogger<KafkaConsumerService<T>> logger)
        {
            _messageHandler = messageHandler;
            _logger = logger;
            _topic = settings.Value.Topic;

            var config = new ConsumerConfig
            {
                BootstrapServers = settings.Value.BootstrapServers,
                GroupId = settings.Value.GroupId,
                AutoOffsetReset = settings.Value.AutoOffsetReset,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    
                    if (consumeResult?.Message == null) continue;

                    var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);
                    
                    if (message != null)
                    {
                        await _messageHandler.HandleAsync(message, stoppingToken);
                        _consumer.Commit(consumeResult);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from topic {Topic}", _topic);
                }
            }
        }

        public override void Dispose()
        {
            _consumer?.Close();
            _consumer?.Dispose();
            base.Dispose();
        }
    }
}