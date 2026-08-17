using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Extensions;
using Fgs.Messaging.Options;
using Fgs.Publisher.Infrastructure.Options;
using Fgs.Publisher.Infrastructure.Outbox;
using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Publisher.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsPublisherInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-publisher-service";
                options.RequiredProviders = ["DATABASE", "RABBITMQ", "ENTRA_EXTERNAL_ID"];
            },
            typeof(RabbitMqOptions));

        services.AddFgsApiSecurity(configuration);
        services.AddFgsUserAuthProfileClient(configuration);

        services.Configure<OutboxSourcesOptions>(configuration.GetSection(OutboxSourcesOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.PostConfigure<RabbitMqOptions>(options =>
        {
            options.ClientProvidedName = "Fgs.Publisher";
            options.AutomaticRecoveryEnabled = true;
        });

        services.AddSingleton<IOutboxStore>(sp =>
        {
            var sourceOptions = sp.GetRequiredService<IOptions<OutboxSourcesOptions>>().Value;
            var config = sp.GetRequiredService<IConfiguration>();
            var credentialProvider = sp.GetService<ICredentialConfigurationProvider>();
            var sources = sourceOptions.GetEnabledSources()
                .Select(source =>
                {
                    var connectionStringName = source.ResolveConnectionStringName();
                    return (ISchemaOutboxSource)new SchemaOutboxStore(
                        source.SourceKey,
                        () => ConnectionStringResolver.ResolveRequired(
                            config,
                            connectionStringName,
                            credentialProvider: credentialProvider),
                        source.Schema,
                        source.Table);
                })
                .ToList();

            return new CompositeOutboxStore(
                sources,
                sp.GetRequiredService<ILogger<CompositeOutboxStore>>());
        });

        services.AddSingleton<IOutboxRoutingResolver, GlobalOutboxRoutingResolver>();
        services.AddFgsRabbitMqPublisher();
        services.AddFgsOutboxProcessor();

        return services;
    }
}
