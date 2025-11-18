using Microsoft.EntityFrameworkCore;
using Pricing.Service.Application.Common.Interfaces;
using Pricing.Service.Infrastructure.Persistence;
using Pricing.Service.Infrastructure.Services;
using Pricing.Service.Application.Features.Pricing.Commands.CreatePricingRule;
using Pricing.Service.Application.Features.Pricing.Queries.CalculatePrice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreatePricingRuleCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CalculatePriceQuery).Assembly);
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register interfaces
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IPricingEngine, PricingEngine>();

// Add EventBus (placeholder implementation)
builder.Services.AddScoped<IEventBus, EventBus>();

// Configure Amadeus API
var amadeusSettings = new AmadeusSettings();
builder.Configuration.GetSection("Amadeus").Bind(amadeusSettings);
builder.Services.AddSingleton(amadeusSettings);

// Add HttpClient for Amadeus API
builder.Services.AddHttpClient<IAmadeusApiService, AmadeusApiService>(client =>
{
    client.BaseAddress = new Uri(amadeusSettings.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

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
