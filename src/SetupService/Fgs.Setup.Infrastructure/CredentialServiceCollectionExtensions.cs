using Amazon.KeyManagementService;
using Amazon.SecretsManager;
using Fgs.Credentials.Aws;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Configuration;
using Fgs.Credentials.Extensions;
using Fgs.Credentials.Options;
using Fgs.Credentials.Redis;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Common.Options;
using Fgs.Setup.Infrastructure.Credentials;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Database.Repositories;
using Fgs.Setup.Infrastructure.Security.Encryption;
using Fgs.Setup.Infrastructure.Security.Kms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Infrastructure;

public static class CredentialServiceCollectionExtensions
{
    public static IServiceCollection AddFgsCredentialConfigurationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IConfigurationBuilder configurationBuilder,
        bool registerCredentialStoreDbContext = false)
    {
        services.AddMemoryCache();

        services.Configure<AwsCredentialsOptions>(configuration.GetSection(AwsCredentialsOptions.SectionName));
        services.Configure<CredentialDistributionOptions>(
            configuration.GetSection(CredentialDistributionOptions.SectionName));
        services.Configure<CredentialConfigurationOptions>(_ => { });
        services.AddSingleton<ConfigureCredentialConfigurationOptions>();
        services.AddSingleton<IConfigureOptions<CredentialConfigurationOptions>>(
            sp => sp.GetRequiredService<ConfigureCredentialConfigurationOptions>());
        services.AddSingleton<IOptions<CredentialConfigurationOptions>, CredentialConfigurationOptionsAccessor>();

        services.AddSingleton<IAmazonKeyManagementService>(CreateKmsClient);
        services.AddSingleton<IKmsService, KmsService>();
        services.AddSingleton<IAesEncryptionService, AesGcmEncryptionService>();
        services.AddScoped<ICredentialEncryptionService, CredentialEncryptionService>();
        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddScoped<ICredentialActorResolver, CredentialActorResolver>();
        services.AddSingleton<ICredentialSecretAccessPolicy, CredentialSecretAccessPolicy>();
        RegisterSecretVault(services);

        var credentialConfigurationHolder = new CredentialConfigurationHolder();
        services.AddSingleton(credentialConfigurationHolder);
        configurationBuilder.AddResolvedCredentialConfiguration(credentialConfigurationHolder);
        configurationBuilder.AddFgsCredentialApplicationConfiguration(credentialConfigurationHolder);

        services.AddSingleton<CredentialOptionsChangeNotifier>();
        services.TryAddSingleton<ICredentialSnapshotRedisCache, CredentialSnapshotRedisCache>();
        services.AddSingleton<ICredentialConfigurationProvider, SetupCredentialConfigurationProvider>();
        services.AddScoped<CredentialConfigurationLoader>();

        if (registerCredentialStoreDbContext)
        {
            RegisterCredentialStoreDbContext(services, configuration);
        }

        return services;
    }

    public static void RegisterCredentialOptionsChangeSource<TOptions>(IServiceCollection services)
        where TOptions : class =>
        Fgs.Credentials.Extensions.CredentialServiceCollectionExtensions
            .RegisterCredentialOptionsChangeSource<TOptions>(services);

    private static void RegisterCredentialStoreDbContext(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = FgsSetupConnectionString.ResolveRequired(configuration)
            ?? throw new InvalidOperationException(
                "ConnectionStrings:FgsSetup is required to load credentials from the credential store.");

        services.AddDbContext<FgsSetupDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsSetupDbContext.MigrationHistorySchema);
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });
            options.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });
    }

    private static void RegisterSecretVault(IServiceCollection services)
    {
        services.TryAddSingleton<IAmazonSecretsManager>(CreateSecretsManagerClient);
        services.AddSingleton<ISecretVault>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
            if (SecretVaultProviders.IsAwsSecretsManager(options.DefaultVaultProvider))
            {
                return new AwsSecretsManagerSecretVault(
                    sp.GetRequiredService<IAmazonSecretsManager>(),
                    sp.GetRequiredService<IOptions<AwsCredentialsOptions>>(),
                    sp.GetRequiredService<ILogger<AwsSecretsManagerSecretVault>>());
            }

            return new NoOpSecretVault();
        });
    }

    private static IAmazonKeyManagementService CreateKmsClient(IServiceProvider sp)
    {
        var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
        var config = new AmazonKeyManagementServiceConfig
        {
            RegionEndpoint = AwsClientCredentialHelper.ResolveRegionEndpoint(options.Region)
        };

        if (AwsClientCredentialHelper.TryResolveExplicitCredentials(
                options.AccessKeyId,
                options.SecretAccessKey,
                out var accessKeyId,
                out var secretAccessKey))
        {
            return new AmazonKeyManagementServiceClient(accessKeyId, secretAccessKey, config);
        }

        return new AmazonKeyManagementServiceClient(config);
    }

    private static IAmazonSecretsManager CreateSecretsManagerClient(IServiceProvider sp)
    {
        var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
        var config = new AmazonSecretsManagerConfig
        {
            RegionEndpoint = AwsClientCredentialHelper.ResolveRegionEndpoint(options.Region)
        };

        if (AwsClientCredentialHelper.TryResolveExplicitCredentials(
                options.AccessKeyId,
                options.SecretAccessKey,
                out var accessKeyId,
                out var secretAccessKey))
        {
            return new AmazonSecretsManagerClient(accessKeyId, secretAccessKey, config);
        }

        return new AmazonSecretsManagerClient(config);
    }
}
