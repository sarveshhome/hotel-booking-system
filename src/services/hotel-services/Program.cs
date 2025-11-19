using Hotel.Service.Application.Common.Interfaces;
using Hotel.Service.Infrastructure.Persistence;
using Hotel.Service.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("HotelServiceDb"));

// Register interfaces
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IEventBus, NoOpEventBus>();

// Register Amadeus API Service
builder.Services.AddHttpClient<IAmadeusApiService, AmadeusApiService>(client =>
{
    client.BaseAddress = new Uri("https://test.api.amadeus.com");
});

// Register Amadeus settings
builder.Services.AddSingleton(new AmadeusSettings
{
    BaseUrl = "https://test.api.amadeus.com",
    ClientId = builder.Configuration["Amadeus:ClientId"] ?? "demo-client-id",
    ClientSecret = builder.Configuration["Amadeus:ClientSecret"] ?? "demo-client-secret"
});

var app = builder.Build();

// Configure minimal pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

// Add health check endpoint
app.MapGet("/health", () => "Hotel Service is running!");

Console.WriteLine("Hotel Service started successfully on ports 5068/5068");
app.Run();