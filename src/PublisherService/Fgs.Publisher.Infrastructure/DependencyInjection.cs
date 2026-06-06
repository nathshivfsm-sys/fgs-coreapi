using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Extensions;
using Fgs.Messaging.Options;
using Fgs.Publisher.Infrastructure.Options;
using Fgs.Publisher.Infrastructure.Outbox;
using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.Publisher.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsPublisherInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        services.Configure<OutboxSourcesOptions>(configuration.GetSection(OutboxSourcesOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.PostConfigure<RabbitMqOptions>(options =>
        {
            options.ClientProvidedName = "Fgs.Publisher";
            options.AutomaticRecoveryEnabled = false;
        });

        services.AddSingleton<IOutboxStore>(sp =>
        {
            var sourceOptions = sp.GetRequiredService<IOptions<OutboxSourcesOptions>>().Value;
            var config = sp.GetRequiredService<IConfiguration>();
            var sources = sourceOptions.Sources
                .Select(source =>
                {
                    var connectionString = config.GetConnectionString(source.ConnectionStringName)
                        ?? throw new InvalidOperationException(
                            $"Connection string '{source.ConnectionStringName}' is not configured for outbox source '{source.SourceKey}'.");

                    return (ISchemaOutboxSource)new SchemaOutboxStore(
                        source.SourceKey,
                        connectionString,
                        source.Schema,
                        source.Table);
                })
                .ToList();

            return new CompositeOutboxStore(sources);
        });

        services.AddSingleton<IOutboxRoutingResolver, GlobalOutboxRoutingResolver>();
        services.AddFgsRabbitMqPublisher();
        services.AddFgsOutboxProcessor();

        return services;
    }
}
