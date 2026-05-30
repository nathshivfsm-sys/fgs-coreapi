using Fgs.Foundation.Extensions;
using Fgs.Messaging.Options;
using Fgs.MultiTenancy.Extensions;
using Fgs.Observability.Extensions;
using Fgs.User.API.Json;
using Fgs.Foundation.Middleware;
using Fgs.User.API.Middleware;
using Fgs.User.API.Swagger;
using Fgs.User.Application;
using Fgs.User.Infrastructure;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Fgs.User.Infrastructure.Persistence.Database.Seed;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

var kmsKeyArnFromEnv = Environment.GetEnvironmentVariable("KMS_KEY_ARN");
if (!string.IsNullOrWhiteSpace(kmsKeyArnFromEnv))
{
    builder.Configuration["AwsCredentials:KmsKeyArn"] = kmsKeyArnFromEnv;
}

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Clear();
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureFgsApi());
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ConfigureFgsApi());
builder.Services.AddFgsUserSwagger();
builder.Services.AddFgsUserApplication();
builder.Services.AddFgsUserInfrastructure(builder.Configuration);
builder.Services.AddFgsMultiTenancy();
builder.Services.AddFgsObservability(builder.Configuration, "fgs-user-service");
builder.Services.AddSingleton<IExceptionStatusMapper, CredentialSecretsExceptionMapper>();

var app = builder.Build();

await ApplyMigrationsAsync(app);
await SeedPlatformTenantAsync(app);

LogRabbitMqEffectiveConfig(app);
ProbeLocalRabbitMqTcpIfDevelopment(app);

app.UseForwardedHeaders();
app.UseFgsFoundationMiddleware(options =>
{
    options.OmitRequestBodyLoggingForPath = path =>
        path.StartsWithSegments("/api/credentials", StringComparison.OrdinalIgnoreCase);
});
app.UseFgsTenantResolution();
if (ShouldUseHttpsRedirection(app.Configuration))
{
    app.UseHttpsRedirection();
}

if (app.Configuration.IsSwaggerEnabled(app.Environment))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FGS User Service v1");
        options.DocumentTitle = "FGS User Service — API";
        options.DisplayRequestDuration();
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFgsHealthChecks();

app.Run();

static async Task SeedPlatformTenantAsync(WebApplication app)
{
    if (!app.Configuration.GetValue("Database:SeedPlatformTenantOnStartup", true))
    {
        return;
    }

    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<PlatformTenantSeeder>();
    await seeder.SeedAsync();
}

static async Task ApplyMigrationsAsync(WebApplication app)
{
    if (!app.Configuration.GetValue("Database:ApplyMigrationsOnStartup", false))
    {
        return;
    }

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FgsUserDbContext>();
    await db.Database.MigrateAsync();
}

static void LogRabbitMqEffectiveConfig(WebApplication app)
{
    var rabbit = app.Services.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    var hasUri = !string.IsNullOrWhiteSpace(rabbit.ConnectionUri);
    app.Logger.LogInformation(
        "RabbitMQ effective configuration (environment {Environment}): " +
        "ConnectionUri set={HasUri}, HostName={HostName}, Port={Port}, SslEnabled={Ssl}, UserName={UserName}, PasswordConfigured={HasPassword}, " +
        "Exchange={Exchange}, EnsureQueuesOnStartup={EnsureQueues}, QueueBindings={BindingCount}. " +
        "If this does not match your Docker broker, clear environment variables RabbitMq__* and user secrets for this section.",
        app.Environment.EnvironmentName,
        hasUri,
        rabbit.HostName,
        rabbit.Port,
        rabbit.SslEnabled,
        rabbit.UserName,
        rabbit.Password.Length > 0,
        rabbit.ExchangeName,
        rabbit.EnsureQueuesOnStartup,
        rabbit.QueueBindings.Count);

    if (app.Environment.IsDevelopment()
        && !hasUri
        && rabbit.HostName.Contains(".mq.", StringComparison.OrdinalIgnoreCase))
    {
        app.Logger.LogWarning(
            "RabbitMQ HostName still looks like Amazon MQ in Development. " +
            "Run with launch profile from Fgs.User.API (see launchSettings.json RabbitMq__*), start Docker (`docker compose up -d` in src/UserService), " +
            "or set RabbitMq__HostName=127.0.0.1.");
    }
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
                "RabbitMQ TCP probe timed out after 4s ({Host}:{Port}). Start the broker: cd src/UserService && docker compose up -d",
                rabbit.HostName,
                rabbit.Port);
            return;
        }

        if (connectTask.IsFaulted)
        {
            app.Logger.LogWarning(
                connectTask.Exception!.GetBaseException(),
                "RabbitMQ TCP probe faulted ({Host}:{Port}).",
                rabbit.HostName,
                rabbit.Port);
            return;
        }

        app.Logger.LogInformation(
            "RabbitMQ TCP probe succeeded ({Host}:{Port}); AMQP handshake can proceed.",
            rabbit.HostName,
            rabbit.Port);
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(
            ex,
            "RabbitMQ TCP probe failed ({Host}:{Port}). No listener — start Docker Compose or fix the port mapping.",
            rabbit.HostName,
            rabbit.Port);
    }
}

static bool ShouldUseHttpsRedirection(IConfiguration configuration) =>
    !string.Equals(configuration["DOTNET_RUNNING_IN_CONTAINER"], "true", StringComparison.OrdinalIgnoreCase)
    && (configuration["ASPNETCORE_URLS"]?.Contains("https://", StringComparison.OrdinalIgnoreCase) == true
        || !string.IsNullOrWhiteSpace(configuration["ASPNETCORE_HTTPS_PORT"]));

public partial class Program;
