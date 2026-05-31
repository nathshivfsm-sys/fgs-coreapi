using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Extensions;
using Fgs.Messaging.Options;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Abstractions.Geo;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Provisioning;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Storage;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Infrastructure.Background;
using Fgs.User.Infrastructure.Common.Geo;
using Fgs.User.Infrastructure.Common.Identity;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Messaging;
using Fgs.User.Infrastructure.Outbox;
using Fgs.User.Infrastructure.Provisioning;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Fgs.User.Infrastructure.Persistence.Database.Seed;
using Fgs.User.Infrastructure.Secrets;
using Fgs.User.Infrastructure.Storage;
using Fgs.User.Infrastructure.Persistence.Database.Repositories;
using Fgs.User.Infrastructure.Persistence.Database.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsUserInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFgsEntraAuthentication(configuration);
        services.AddScoped<IFgsClaimsEnricher, DbFgsClaimsEnricher>();
        services.AddScoped<IFgsUserRoleResolver, FgsUserRoleResolver>();
        services.AddScoped<IFgsUserProfileResolver, FgsUserProfileResolver>();
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.PostConfigure<RabbitMqOptions>(options =>
        {
            options.ClientProvidedName = "Fgs.User";
            options.AutomaticRecoveryEnabled = false;
        });
        services.Configure<EntraExternalIdOptions>(configuration.GetSection(EntraExternalIdOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<SignupLocaleOptions>(configuration.GetSection(SignupLocaleOptions.SectionName));
        services.AddAwsCredentialsServices(configuration);
        services.AddScoped<ISecretsManagerService, AwsSecretsManagerService>();
        services.AddScoped<ISecretCache, MemorySecretCache>();
        services.AddScoped<ICredentialAuditWriter, CredentialAuditWriter>();
        services.AddScoped<ICredentialSecretResolver, CredentialSecretResolver>();
        services.AddScoped<ICredentialPayloadDeserializer, CredentialPayloadDeserializer>();
        services.AddScoped<ICredentialConnectionStringBuilder, CredentialConnectionStringBuilder>();
        services.AddScoped<ICredentialSecretNameBuilder, CredentialSecretNameBuilder>();
        services.Configure<TenantProvisioningOptions>(configuration.GetSection(TenantProvisioningOptions.SectionName));
        services.Configure<RabbitMqConsumerOptions>(configuration.GetSection(RabbitMqConsumerOptions.SectionName));

        var connectionString = FgsUserConnectionString.ResolveRequired(configuration);

        services.AddDbContext<FgsUserDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsUserDbContext.MigrationHistorySchema);
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });
            // SQL-script migrations may drift from the snapshot; do not block startup or drop constraints.
            options.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<IOutboxStore, GloOutboxStore>();
        services.AddSingleton<IOutboxRoutingResolver, UserOutboxRoutingResolver>();
        services.AddFgsRabbitMqPublisher();
        services.AddFgsOutboxProcessor();
        services.AddScoped<PlatformTenantSeeder>();
        services.AddScoped<IAddressLocaleResolver, AddressLocaleResolver>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IEmailNormalizer, EmailNormalizer>();
        services.AddSingleton<IInvitationTokenService, InvitationTokenService>();        services.AddSingleton<RabbitMqTopologyService>();
        services.AddSingleton<ITenantSeedDatabaseConnectionFactory>(sp =>
            new TenantSeedDatabaseConnectionFactory(
                FgsUserConnectionString.ResolveRequired(configuration),
                sp.GetRequiredService<IOptions<TenantProvisioningOptions>>()));
        services.AddScoped<ITenantDataSeedingEngine, TenantDataSeedingEngine>();
        services.AddScoped<ITenantS3BucketProvisioner, TenantS3BucketProvisioner>();
        services.AddScoped<ITenantProvisioningOrchestrator, TenantProvisioningOrchestrator>();
        services.AddSingleton<IS3ObjectKeyBuilder, S3ObjectKeyBuilder>();
        services.AddHttpClient<IEntraExternalIdService, EntraExternalIdService>();
        services.AddHostedService<TenantProvisionConsumerService>();

        return services;
    }
}
