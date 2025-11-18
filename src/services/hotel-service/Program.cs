using Hotel.Service.Infrastructure.Services;
using Hotel.Service.Infrastructure.Persistence;
using Hotel.Service.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Add Entity Framework (with fallback for development)
try
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    // Register application interfaces
    builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
}
catch
{
    // For development without SQL Server
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("HotelServiceDb"));
    
    builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
}

builder.Services.AddScoped<IEventBus, EventBusService>();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Hotel Service API", 
        Version = "v1",
        Description = "Microservice for hotel search and booking"
    });
});

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
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Service API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine($"Hotel Service is running on: {builder.Configuration["Urls"]}");
Console.WriteLine("Swagger UI available at: https://localhost:7001/swagger");

app.Run();