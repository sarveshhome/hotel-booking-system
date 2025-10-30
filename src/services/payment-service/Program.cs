using Microsoft.EntityFrameworkCore;
using Payment.Service.Application.Common.Interfaces;
using Payment.Service.Infrastructure.Persistence;
using Payment.Service.Application.Features.Payments.Commands.ProcessPayment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(ProcessPaymentCommand).Assembly);
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register interfaces
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

builder.services.AddKafkaProducer(options =>
{
    options.BootstrapServers = configuration["Kafka:BootstrapServers"];
    options.ClientId = "payment-service";
});

// Add EventBus (placeholder implementation)
builder.Services.AddScoped<IEventBus, EventBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Placeholder EventBus implementation
public class EventBus : IEventBus
{
    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        // TODO: Implement actual event publishing (e.g., to Kafka, RabbitMQ, etc.)
        Console.WriteLine($"Event published: {@event}");
        return Task.CompletedTask;
    }
}
