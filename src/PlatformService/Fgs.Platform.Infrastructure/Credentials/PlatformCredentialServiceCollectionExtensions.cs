using Fgs.Messaging.Options;
using Fgs.Platform.Infrastructure.Integrations.SendGrid;
using Fgs.Platform.Infrastructure.Options;
using Fgs.User.Application.Abstractions.Credentials;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Fgs.Platform.Infrastructure.Credentials;

public static class PlatformCredentialServiceCollectionExtensions
{
    public static IServiceCollection AddPlatformResolvedCredentialConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        IConfigurationBuilder configurationBuilder)
    {
        services.Configure<UserServiceCredentialClientOptions>(
            configuration.GetSection(UserServiceCredentialClientOptions.SectionName));

        var credentialConfigurationHolder = new CredentialConfigurationHolder();
        services.AddSingleton(credentialConfigurationHolder);
        configurationBuilder.Add(new CredentialApplicationConfigurationSource(credentialConfigurationHolder));

        services.AddSingleton<CredentialOptionsChangeNotifier>();
        services.AddSingleton<RabbitMqConsumerStartupGate>();
        services.AddSingleton<RabbitMqOptionsResolver>();
        services.AddSingleton<IPostConfigureOptions<RabbitMqOptions>, RabbitMqCredentialOptionsPostConfigure>();
        services.AddSingleton<ICredentialConfigurationProvider, PlatformCredentialConfigurationProvider>();
        services.AddSingleton<RemoteCredentialConfigurationLoader>();

        services.AddSingleton<IOptionsChangeTokenSource<SendGridOptions>, CredentialOptionsChangeTokenSource<SendGridOptions>>();
        services.AddSingleton<IOptionsChangeTokenSource<RabbitMqOptions>, CredentialOptionsChangeTokenSource<RabbitMqOptions>>();

        services
            .AddRefitClient<IUserCredentialConfigurationClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<UserServiceCredentialClientOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

        return services;
    }
}
