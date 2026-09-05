using Fgs.Contracts.Observability;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.HealthChecks;
using Fgs.Messaging.Options;
using Fgs.Messaging.Outbox;
using Fgs.Messaging.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

    /// <summary>
    /// Registers a service-owned outbox poller that publishes only the configured local table(s).
    /// Default transport is RabbitMQ; call <see cref="OutboxPublisherBuilder.UsePublisher{TPublisher}"/> to swap (e.g. SQS).
    /// </summary>
    public static IServiceCollection AddFgsOutboxPublisher(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<OutboxPublisherBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(configure);

        var builder = new OutboxPublisherBuilder();
        configure(builder);

        if (builder.Sources.Count == 0)
        {
            throw new InvalidOperationException(
                "AddFgsOutboxPublisher requires at least one AddSource registration.");
        }

        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        if (!string.IsNullOrWhiteSpace(builder.ClientProvidedName))
        {
            var clientName = builder.ClientProvidedName;
            var automaticRecovery = builder.AutomaticRecoveryEnabled;
            services.PostConfigure<RabbitMqOptions>(options =>
            {
                options.ClientProvidedName = clientName;
                options.AutomaticRecoveryEnabled = automaticRecovery;
                options.EnsureQueuesOnStartup = false;
            });
        }
        else
        {
            services.PostConfigure<RabbitMqOptions>(options =>
            {
                options.EnsureQueuesOnStartup = false;
            });
        }

        var sourceRegistrations = builder.Sources.ToList();
        services.AddSingleton<IOutboxStore>(sp =>
        {
            var sources = sourceRegistrations
                .Select(source => (ISchemaOutboxSource)new SchemaOutboxStore(
                    source.SourceKey,
                    () => source.ConnectionStringFactory(sp),
                    source.Schema,
                    source.Table))
                .ToList();

            return new CompositeOutboxStore(
                sources,
                sp.GetRequiredService<ILogger<CompositeOutboxStore>>());
        });

        services.TryAddSingleton<IOutboxDestinationResolver, ContractOutboxDestinationResolver>();

        if (builder.CustomPublisherType is { } customPublisherType)
        {
            services.TryAddSingleton(typeof(IIntegrationEventPublisher), customPublisherType);
        }
        else
        {
            services.AddFgsRabbitMqPublisher();
            services.TryAddSingleton<IIntegrationEventPublisher, RabbitMqIntegrationEventPublisher>();
        }

        services.AddFgsOutboxProcessor();
        return services;
    }
}
