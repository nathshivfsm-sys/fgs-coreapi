using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fgs.Messaging.HealthChecks;

public sealed class RabbitMqReadyHealthCheck(
    IOptionsMonitor<RabbitMqOptions> optionsMonitor) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var options = optionsMonitor.CurrentValue;
        try
        {
            var factory = new ConnectionFactory
            {
                Uri = RabbitMqBrokerUriResolver.Resolve(options),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            return connection.IsOpen
                ? HealthCheckResult.Healthy("RabbitMQ is reachable.")
                : HealthCheckResult.Unhealthy("RabbitMQ connection is not open.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("RabbitMQ is unreachable.", ex);
        }
    }
}

public static class RabbitMqHealthCheckExtensions
{
    public static IServiceCollection AddFgsRabbitMqReadyCheck(this IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<RabbitMqReadyHealthCheck>(
            "rabbitmq",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["ready"]);
        return services;
    }
}
