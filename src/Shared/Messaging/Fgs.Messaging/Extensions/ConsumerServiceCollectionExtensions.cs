using Fgs.Messaging.Consumer;
using Fgs.Messaging.HealthChecks;
using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fgs.Messaging.Extensions;

public static class ConsumerServiceCollectionExtensions
{
    public static IServiceCollection AddFgsRabbitMqConsumerFramework(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ConsumerOptions>(configuration.GetSection(ConsumerOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.PostConfigure<RabbitMqOptions>(options =>
        {
            options.ClientProvidedName = "Fgs.Consumer";
            options.AutomaticRecoveryEnabled = true;
        });

        services.TryAddSingleton<Fgs.Contracts.Observability.IFgsMetrics>(
            Fgs.Contracts.Observability.NoOpFgsMetrics.Instance);
        services.AddFgsRabbitMqConnectionFactory();
        services.AddFgsRabbitMqReadyCheck();
        services.TryAddSingleton<IConsumerIdempotencyStore, DistributedCacheConsumerIdempotencyStore>();
        services.AddSingleton<ConsumerRoutingRegistry>(sp =>
        {
            var registry = new ConsumerRoutingRegistry();
            foreach (var registration in sp.GetServices<IConsumerRouteRegistration>())
            {
                registration.Apply(registry);
            }

            return registry;
        });
        services.AddSingleton<SubscriptionManager>();
        services.AddSingleton<ConsumerRetryPolicy>();
        services.AddScoped<MessageDispatcher>();
        services.AddHostedService<ConsumerHost>();

        return services;
    }

    /// <summary>
    /// Registers the no-op store (tests / hosts without distributed cache). Prefer
    /// <see cref="DistributedCacheConsumerIdempotencyStore"/> in production.
    /// </summary>
    public static IServiceCollection AddFgsNoOpConsumerIdempotency(this IServiceCollection services)
    {
        services.RemoveAll<IConsumerIdempotencyStore>();
        services.AddSingleton<IConsumerIdempotencyStore, NoOpConsumerIdempotencyStore>();
        return services;
    }

    public static IServiceCollection AddConsumerRouting<TMessage>(
        this IServiceCollection services,
        string routingKey,
        Func<TMessage, ConsumerMessageContext, object> commandFactory)
        where TMessage : class
    {
        services.AddSingleton<IConsumerRouteRegistration>(
            new ConsumerRouteRegistration<TMessage>(routingKey, commandFactory));
        return services;
    }
}
