using Fgs.Audit.Infrastructure.Database;
using Fgs.Contracts.Clients;
using Fgs.Messaging.Extensions;
using Fgs.Messaging.Options;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Infrastructure.Common.Options;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Messaging;
using Fgs.Setup.Infrastructure.Outbox;
using Fgs.Setup.Infrastructure.Provisioning;
using Fgs.Foundation.Extensions;
using Fgs.Messaging.Abstractions;
using Fgs.Security.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Fgs.Setup.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsSetupInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsFoundation();
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.Configure<TenantProvisioningOptions>(configuration.GetSection(TenantProvisioningOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<RabbitMqConsumerOptions>(configuration.GetSection(RabbitMqConsumerOptions.SectionName));
        services.AddSingleton<RabbitMqTopologyService>();

        var connectionString = FgsSetupConnectionString.ResolveRequired(configuration);
        services.AddDbContext<FgsSetupDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsSetupDbContext.MigrationHistorySchema);
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        var auditConnectionString = configuration.GetConnectionString("FgsAudit")
            ?? connectionString;
        services.AddDbContext<FgsAuditDbContext>((_, options) =>
        {
            options.UseNpgsql(auditConnectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsAuditDbContext.MigrationHistorySchema));
        });

        var userBaseUrl = configuration["UserService:BaseUrl"] ?? "http://user-service:5001";
        services.AddRefitClient<IUserTenantClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(userBaseUrl));

        var fileBaseUrl = configuration["FileService:BaseUrl"] ?? "http://file-service:5004";
        services.AddRefitClient<IFileTenantClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(fileBaseUrl));

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ITenantSeedDatabaseConnectionFactory>(sp =>
            new TenantSeedDatabaseConnectionFactory(
                connectionString,
                sp.GetRequiredService<IOptions<TenantProvisioningOptions>>()));
        services.AddScoped<ITenantDataSeedingEngine, TenantDataSeedingEngine>();
        services.AddScoped<ITenantProvisioningOrchestrator, TenantProvisioningOrchestrator>();
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<IOutboxStore, GloOutboxStore>();
        services.AddFgsRabbitMqPublisher();
        services.AddFgsOutboxProcessor();
        services.AddHostedService<Background.TenantProvisionConsumerService>();
        CredentialServiceCollectionExtensions.AddFgsCredentialConfigurationServices(
            services,
            configuration,
            configuration,
            registerCredentialStoreDbContext: false);

        return services;
    }
}
