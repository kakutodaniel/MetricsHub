using FluentValidation;
using MetricsHub.Application.Modules.Events.DTOs.Validators;
using MetricsHub.Application.Modules.Events.Queries;
using MetricsHub.Application.Modules.Events.Services;
using MetricsHub.Application.Modules.Events.Services.Interfaces;
using MetricsHub.Infrastructure.Persistence;
using MetricsHub.Infrastructure.Persistence.Repositories;
using MetricsHub.Infrastructure.Queries;
using MetricsHub.Presentation.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Setup Database
var dbPath = Path.Combine(AppContext.BaseDirectory, "demo.db");
builder.Services.AddDbContext<MetricsHubDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddValidatorsFromAssemblyContaining<PulsePayloadValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<AlertPayloadValidator>();

builder.Services.AddScoped<IIngestionEventService, IngestionEventService>();
builder.Services.AddScoped<IIngestionEventRepository, IngestionEventRepository>();
builder.Services.AddScoped<IIngestionEventQueries, IngestionEventQueries>();

builder.Services.AddHealthChecks()
    .AddSqlite(
        connectionString: "Data Source=demo.db",
        name: "sqlite",
        tags: new[] { "ready" });

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("LimitByIp", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString()
                 ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString()
            })
        });

        await context.Response.WriteAsync(result);
    }
});

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MetricsHubDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
