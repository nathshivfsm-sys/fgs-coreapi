using Fgs.Contracts.Clients;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Configuration;
using Fgs.Credentials.Options;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Credentials.Extensions;

public static class CredentialServiceCollectionExtensions
{
    public static IServiceCollection AddFgsRemoteCredentialConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        IConfigurationBuilder configurationBuilder,
        string setupBaseUrlKey = "SetupService:BaseUrl",
        string setupDefaultBaseUrl = "http://setup-service:5004",
        bool registerSetupClient = true)
    {
        services.Configure<CredentialDistributionOptions>(
            configuration.GetSection(CredentialDistributionOptions.SectionName));
        services.Configure<CredentialConsumerOptions>(
            configuration.GetSection(CredentialConsumerOptions.SectionName));

        var holder = new CredentialConfigurationHolder();
        services.AddSingleton(holder);
        services.AddSingleton<CredentialOptionsChangeNotifier>();
        services.AddSingleton<RemoteCredentialConfigurationLoader>();
        services.AddSingleton<ICredentialConfigurationProvider, CredentialConfigurationProvider>();

        configurationBuilder.Add(new CredentialApplicationConfigurationSource(holder));

        if (registerSetupClient)
        {
            services.AddFgsRefitClient<ISetupClient>(configuration, setupBaseUrlKey, setupDefaultBaseUrl);
        }

        return services;
    }

    public static void RegisterCredentialOptionsChangeSource<TOptions>(IServiceCollection services)
        where TOptions : class =>
        services.AddSingleton<Microsoft.Extensions.Options.IOptionsChangeTokenSource<TOptions>,
            CredentialOptionsChangeTokenSource<TOptions>>();
}
