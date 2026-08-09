using Fgs.Contracts.Observability;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.HealthChecks;
using Fgs.Messaging.Options;
using Fgs.Messaging.Outbox;
using Fgs.Messaging.RabbitMq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFgsRabbitMqPublisher(this IServiceCollection services)
    {
        services.TryAddSingleton<IFgsMetrics>(NoOpFgsMetrics.Instance);
        services.AddSingleton<RabbitMqPublisher>();
        services.AddSingleton<IRabbitMqPublisher>(sp => sp.GetRequiredService<RabbitMqPublisher>());
        services.AddFgsRabbitMqReadyCheck();
        return services;
    }

    public static IServiceCollection AddFgsRabbitMqConnectionFactory(this IServiceCollection services)
    {
        services.TryAddSingleton<IRabbitMqEffectiveOptionsProvider, OptionsMonitorRabbitMqEffectiveOptionsProvider>();
        services.AddSingleton<RabbitMqConnectionFactory>();
        return services;
    }

    public static IServiceCollection AddFgsOutboxProcessor(this IServiceCollection services)
    {
        services.AddScoped<OutboxBatchProcessor>();
        services.AddHostedService<OutboxPollingBackgroundService>();
        return services;
    }
}
