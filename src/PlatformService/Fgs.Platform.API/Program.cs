using Fgs.Platform.API.Middleware;
using Fgs.Platform.API.Swagger;
using Fgs.Platform.Application;
using Fgs.Platform.Infrastructure;
using Fgs.Platform.Infrastructure.Database;
using Fgs.Platform.Infrastructure.Database.Seed;
using Fgs.Platform.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFgsPlatformSwagger();
builder.Services.AddFgsPlatformApplication();
builder.Services.AddFgsPlatformInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();

await ApplyMigrationsAsync(app);
await SeedCommunicationTemplatesAsync(app);
LogRabbitMqEffectiveConfig(app);
ProbeLocalRabbitMqTcpIfDevelopment(app);

app.UseMiddleware<CorrelationIdMiddleware>();
if (ShouldUseHttpsRedirection(app.Configuration))
{
    app.UseHttpsRedirection();
}

if (app.Configuration.IsSwaggerEnabled(app.Environment))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FGS Platform Service v1");
        options.DocumentTitle = "FGS Platform Service — API";
    });
}

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    if (!app.Configuration.GetValue("Database:ApplyMigrationsOnStartup", true))
    {
        return;
    }

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FgsPlatformDbContext>();
    await db.Database.MigrateAsync();
}

static async Task SeedCommunicationTemplatesAsync(WebApplication app)
{
    if (!app.Configuration.GetValue("Database:SeedCommunicationTemplatesOnStartup", true))
    {
        return;
    }

    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<CommunicationTemplateSeeder>();
    await seeder.SeedAsync();
}

static void LogRabbitMqEffectiveConfig(WebApplication app)
{
    var rabbit = app.Services.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    var routingKeys = string.Join(", ", rabbit.QueueBindings.Select(b => b.RoutingKey));
    app.Logger.LogInformation(
        "RabbitMQ consumer (Platform): HostName={HostName}, Port={Port}, Exchange={Exchange}, ConsumeQueue={Queue}, DLQ={Dlq}, RoutingKeys=[{RoutingKeys}]",
        rabbit.HostName,
        rabbit.Port,
        rabbit.ExchangeName,
        rabbit.NotificationQueueName,
        rabbit.DeadLetterQueueName,
        routingKeys);
}

static void ProbeLocalRabbitMqTcpIfDevelopment(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        return;
    }

    var rabbit = app.Services.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    if (rabbit.HostName is not ("127.0.0.1" or "localhost"))
    {
        return;
    }

    try
    {
        using var client = new TcpClient();
        var connectTask = client.ConnectAsync(rabbit.HostName, rabbit.Port);
        if (!connectTask.Wait(TimeSpan.FromSeconds(4)))
        {
            app.Logger.LogWarning(
                "RabbitMQ TCP probe timed out ({Host}:{Port}). Start broker via docker compose in src/PlatformService.",
                rabbit.HostName,
                rabbit.Port);
            return;
        }

        app.Logger.LogInformation("RabbitMQ TCP probe succeeded ({Host}:{Port}).", rabbit.HostName, rabbit.Port);
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "RabbitMQ TCP probe failed ({Host}:{Port}).", rabbit.HostName, rabbit.Port);
    }
}

static bool ShouldUseHttpsRedirection(IConfiguration configuration) =>
    !string.Equals(configuration["DOTNET_RUNNING_IN_CONTAINER"], "true", StringComparison.OrdinalIgnoreCase)
    && (configuration["ASPNETCORE_URLS"]?.Contains("https://", StringComparison.OrdinalIgnoreCase) == true
        || !string.IsNullOrWhiteSpace(configuration["ASPNETCORE_HTTPS_PORT"]));

public partial class Program;
