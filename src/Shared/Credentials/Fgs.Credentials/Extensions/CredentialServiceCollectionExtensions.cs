using System.Reflection;
using Fgs.Contracts.Clients;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Configuration;
using Fgs.Credentials.Http;
using Fgs.Credentials.Options;
using Fgs.Credentials.Redis;
using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Fgs.Security.UserAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials.Extensions;

public static class CredentialServiceCollectionExtensions
{
    public static IServiceCollection AddFgsStandardInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration,
        string serviceName,
        params string[] requiredProviders) =>
        services
            .AddFgsCredentialConsumer(
                configuration,
                configuration,
                options =>
                {
                    options.ServiceName = serviceName;
                    // JWT bearer is registered via AddFgsApiSecurity for all standard API hosts.
                    options.RequiredProviders = requiredProviders
                        .Append("ENTRA_EXTERNAL_ID")
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                })
            .AddFgsApiSecurity(configuration)
            .AddFgsUserAuthProfileClient(configuration);

    public static IServiceCollection AddFgsCredentialConsumer(
        this IServiceCollection services,
        IConfiguration configuration,
        ConfigurationManager configurationBuilder,
        Action<FgsCredentialConsumerOptions> configure,
        params Type[] optionTypesToReload)
    {
        var consumerOptions = new FgsCredentialConsumerOptions();
        configure(consumerOptions);

        services.Configure<CredentialConsumerOptions>(options =>
        {
            options.ServiceName = consumerOptions.ServiceName;
            options.RequiredProviders = consumerOptions.RequiredProviders;
        });

        services.AddFgsRemoteCredentialConfiguration(
            configuration,
            configurationBuilder,
            consumerOptions.SetupBaseUrlKey,
            consumerOptions.SetupDefaultBaseUrl,
            consumerOptions.RegisterSetupClient);

        foreach (var optionType in optionTypesToReload)
        {
            RegisterCredentialOptionsChangeSource(services, optionType);
        }

        return services;
    }

    private static void RegisterCredentialOptionsChangeSource(IServiceCollection services, Type optionType)
    {
        var method = typeof(CredentialServiceCollectionExtensions)
            .GetMethod(
                nameof(RegisterCredentialOptionsChangeSource),
                BindingFlags.Public | BindingFlags.Static,
                binder: null,
                types: [typeof(IServiceCollection)],
                modifiers: null)!
            .MakeGenericMethod(optionType);
        method.Invoke(null, [services]);
    }

    public static IServiceCollection AddFgsRemoteCredentialConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        IConfigurationBuilder configurationBuilder,
        string setupBaseUrlKey = "SetupService:BaseUrl",
        string setupDefaultBaseUrl = "http://setup-service:5004",
        bool registerSetupClient = true)
    {
        if (configuration is ConfigurationManager configurationManager)
        {
            configurationManager.ApplyFgsAwsCredentialEnvironmentVariables();
        }

        services.Configure<CredentialDistributionOptions>(
            configuration.GetSection(CredentialDistributionOptions.SectionName));
        services.Configure<CredentialConsumerOptions>(
            configuration.GetSection(CredentialConsumerOptions.SectionName));

        var holder = new CredentialConfigurationHolder();
        services.AddSingleton(holder);
        services.AddSingleton<CredentialOptionsChangeNotifier>();
        services.AddSingleton<RemoteCredentialConfigurationLoader>();
        services.AddSingleton<ICredentialConfigurationProvider, CredentialConfigurationProvider>();
        services.TryAddSingleton<ICredentialSnapshotRedisCache, CredentialSnapshotRedisCache>();
        services.AddHostedService<CredentialSnapshotReloadHostedService>();

        configurationBuilder.Add(new CredentialApplicationConfigurationSource(holder));

        if (registerSetupClient)
        {
            services.AddFgsSetupClient(configuration, setupBaseUrlKey, setupDefaultBaseUrl);
        }

        return services;
    }

    public static IServiceCollection AddFgsSetupClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string setupBaseUrlKey = "SetupService:BaseUrl",
        string setupDefaultBaseUrl = "http://setup-service:5004")
    {
        return services.AddFgsInternalServiceRefitClient<ISetupClient>(
            configuration,
            setupBaseUrlKey,
            setupDefaultBaseUrl);
    }

    public static IServiceCollection AddFgsUserAuthProfileClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string userServiceBaseUrlKey = "UserService:BaseUrl",
        string userServiceDefaultBaseUrl = "http://user-service:5001")
    {
        services.TryAddScoped<IUserAuthProfileSource, RemoteUserAuthProfileSource>();

        return services.AddFgsInternalServiceRefitClient<IUserAuthProfileClient>(
            configuration,
            userServiceBaseUrlKey,
            userServiceDefaultBaseUrl);
    }

    public static IServiceCollection AddFgsInternalServiceRefitClient<TClient>(
        this IServiceCollection services,
        IConfiguration configuration,
        string baseUrlConfigurationKey,
        string? defaultBaseUrl = null)
        where TClient : class
    {
        services.TryAddTransient<InternalServiceKeyDelegatingHandler>();
        services.AddFgsRefitClient<TClient>(
            configuration,
            baseUrlConfigurationKey,
            defaultBaseUrl,
            builder =>
            {
                builder.AddHttpMessageHandler<InternalServiceKeyDelegatingHandler>();
                // Also stamp DefaultRequestHeaders so S2S calls work even if a handler
                // in the pipeline fails to run (matches explicit [Header] usage on ISetupClient).
                builder.ConfigureHttpClient((sp, client) =>
                    ApplyInternalServiceDefaultHeaders(client, sp, configuration));
            });

        return services;
    }

    private static void ApplyInternalServiceDefaultHeaders(
        HttpClient client,
        IServiceProvider sp,
        IConfiguration configuration)
    {
        var distribution = sp.GetService<IOptions<CredentialDistributionOptions>>()?.Value;
        var serviceKey = FirstNonEmpty(
            distribution?.InternalServiceKey,
            configuration[$"{CredentialDistributionOptions.SectionName}:InternalServiceKey"],
            Environment.GetEnvironmentVariable("CredentialDistribution__InternalServiceKey"),
            Environment.GetEnvironmentVariable("CREDENTIAL_DISTRIBUTION_KEY"));

        if (!string.IsNullOrWhiteSpace(serviceKey)
            && !client.DefaultRequestHeaders.Contains(InternalServiceHeaders.ServiceKey))
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(
                InternalServiceHeaders.ServiceKey,
                serviceKey);
        }

        var consumer = sp.GetService<IOptions<CredentialConsumerOptions>>()?.Value;
        var serviceName = FirstNonEmpty(
            consumer?.ServiceName,
            configuration[$"{CredentialConsumerOptions.SectionName}:ServiceName"],
            Environment.GetEnvironmentVariable("CredentialConsumer__ServiceName"));

        if (!string.IsNullOrWhiteSpace(serviceName)
            && !client.DefaultRequestHeaders.Contains(InternalServiceHeaders.ServiceName))
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(
                InternalServiceHeaders.ServiceName,
                serviceName);
        }
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return null;
    }

    public static void RegisterCredentialOptionsChangeSource<TOptions>(IServiceCollection services)
        where TOptions : class =>
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                Microsoft.Extensions.Options.IOptionsChangeTokenSource<TOptions>,
                CredentialOptionsChangeTokenSource<TOptions>>());
}
