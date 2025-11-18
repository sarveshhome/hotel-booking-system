using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

// Add Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
        c.SwaggerEndpoint("http://localhost:7001/swagger/v1/swagger.json", "Hotel Service v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Use Ocelot middleware
await app.UseOcelot();

Console.WriteLine("API Gateway is running on: http://localhost:5022");
Console.WriteLine("Swagger UI available at: http://localhost:5022/swagger");

app.Run();
