using Amazon;
using Amazon.KeyManagementService;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Common.Options;
using Fgs.Setup.Infrastructure.Common.Options;
using Fgs.Setup.Infrastructure.Credentials;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Database.Repositories;
using Fgs.Setup.Infrastructure.Security.Encryption;
using Fgs.Setup.Infrastructure.Security.Kms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        var credentialConfigurationHolder = new CredentialConfigurationHolder();
        services.AddSingleton(credentialConfigurationHolder);
        configurationBuilder.AddResolvedCredentialConfiguration(credentialConfigurationHolder);
        configurationBuilder.Add(new CredentialApplicationConfigurationSource(credentialConfigurationHolder));

        services.AddSingleton<CredentialOptionsChangeNotifier>();
        services.AddSingleton<ICredentialConfigurationProvider, CredentialConfigurationProvider>();
        services.AddScoped<CredentialConfigurationLoader>();

        if (registerCredentialStoreDbContext)
        {
            RegisterCredentialStoreDbContext(services, configuration);
        }

        return services;
    }

    public static void RegisterCredentialOptionsChangeSource<TOptions>(IServiceCollection services)
        where TOptions : class =>
        services.AddSingleton<IOptionsChangeTokenSource<TOptions>, CredentialOptionsChangeTokenSource<TOptions>>();

    private static void RegisterCredentialStoreDbContext(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FgsSetup")
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

    private static IAmazonKeyManagementService CreateKmsClient(IServiceProvider sp)
    {
        var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
        var config = new AmazonKeyManagementServiceConfig
        {
            RegionEndpoint = ResolveRegionEndpoint(options.Region)
        };

        if (TryResolveExplicitCredentials(options, out var accessKeyId, out var secretAccessKey))
        {
            return new AmazonKeyManagementServiceClient(accessKeyId, secretAccessKey, config);
        }

        return new AmazonKeyManagementServiceClient(config);
    }

    private static bool TryResolveExplicitCredentials(
        AwsCredentialsOptions options,
        out string accessKeyId,
        out string secretAccessKey)
    {
        accessKeyId = options.AccessKeyId
            ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")
            ?? string.Empty;
        secretAccessKey = options.SecretAccessKey
            ?? Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
            ?? string.Empty;

        return !string.IsNullOrWhiteSpace(accessKeyId)
            && !string.IsNullOrWhiteSpace(secretAccessKey);
    }

    private static RegionEndpoint ResolveRegionEndpoint(string? region) =>
        RegionEndpoint.GetBySystemName(string.IsNullOrWhiteSpace(region) ? "us-east-1" : region);
}
