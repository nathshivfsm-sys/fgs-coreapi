using Fgs.Foundation.Api;
using Fgs.Foundation.Extensions;
using Fgs.Messaging.Options;
using Fgs.MultiTenancy.Extensions;
using Fgs.Observability.Extensions;
using Fgs.Notification.Application;
using Fgs.Notification.Infrastructure;
using Fgs.Notification.Infrastructure.Database;
using Fgs.Notification.Infrastructure.Database.Seed;
using Fgs.User.Application.Abstractions.Credentials;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFgsApiVersioning();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureFgsApi());
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ConfigureFgsApi());
builder.Services.AddFgsSwagger(options =>
{
    options.Title = "FGS Notification Service";
    options.Description =
        "Shared platform capabilities: notifications (email/SMS/push), integrations, audit, background jobs, and reporting foundations.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
});
builder.Services.AddFgsNotificationApplication();
builder.Services.AddFgsNotificationInfrastructure(builder.Configuration);
builder.Services.AddFgsMultiTenancy();
builder.Services.AddFgsObservability(builder.Configuration, "fgs-notification-service");

var app = builder.Build();

await ApplyMigrationsAsync(app);
await SeedCommunicationTemplatesAsync(app);
LogRabbitMqEffectiveConfig(app);
ProbeLocalRabbitMqTcpIfDevelopment(app);

app.UseFgsFoundationMiddleware();
if (ShouldUseHttpsRedirection(app.Configuration))
{
    app.UseHttpsRedirection();
}

app.UseFgsSwagger();

app.UseAuthentication();
app.UseFgsTenantResolution();
app.UseAuthorization();
app.MapControllers();
app.MapFgsHealthChecks();

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    if (!app.Configuration.GetValue("Database:ApplyMigrationsOnStartup", true))
    {
        return;
    }

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FgsNotificationDbContext>();
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
    var rabbit = app.Services.GetRequiredService<IOptionsMonitor<RabbitMqOptions>>().CurrentValue;
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

    var rabbit = app.Services.GetRequiredService<IOptionsMonitor<RabbitMqOptions>>().CurrentValue;
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
                "RabbitMQ TCP probe timed out ({Host}:{Port}). Start broker via docker compose in src/NotificationService.",
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
