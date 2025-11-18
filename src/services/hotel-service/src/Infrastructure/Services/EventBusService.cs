using Hotel.Service.Application.Common.Interfaces;
using Confluent.Kafka;
using System.Text.Json;

namespace Hotel.Service.Infrastructure.Services;

public class EventBusService : IEventBus
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<EventBusService> _logger;

    public EventBusService(ILogger<EventBusService> logger)
    {
        _logger = logger;
        
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = "hotel-service"
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var eventName = typeof(T).Name;
            var message = JsonSerializer.Serialize(@event);

            var kafkaMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            };

            await _producer.ProduceAsync($"hotel-service.{eventName}", kafkaMessage, cancellationToken);
            _logger.LogInformation("Published event {EventName}", eventName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType}", typeof(T).Name);
            throw;
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}