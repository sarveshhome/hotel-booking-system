using Hotel.Service.Application.Common.Interfaces;

namespace Hotel.Service.Infrastructure.Services;

public class NoOpEventBus : IEventBus
{
    private readonly ILogger<NoOpEventBus> _logger;

    public NoOpEventBus(ILogger<NoOpEventBus> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("NoOpEventBus: Event {EventType} would be published (Kafka not available)", typeof(T).Name);
        return Task.CompletedTask;
    }
}