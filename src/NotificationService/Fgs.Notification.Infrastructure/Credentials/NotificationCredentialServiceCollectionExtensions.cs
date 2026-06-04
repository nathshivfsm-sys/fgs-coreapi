using Fgs.Messaging.Options;
using Fgs.Notification.Infrastructure.Integrations.SendGrid;
using Fgs.Notification.Infrastructure.Options;
using Fgs.Setup.Application.Abstractions.Credentials;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Extensions;

namespace Fgs.Notification.Infrastructure.Credentials;

public static class NotificationCredentialServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationResolvedCredentialConfiguration(
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
        services.AddSingleton<ICredentialConfigurationProvider, NotificationCredentialConfigurationProvider>();
        services.AddSingleton<RemoteCredentialConfigurationLoader>();

        services.AddSingleton<IOptionsChangeTokenSource<SendGridOptions>, CredentialOptionsChangeTokenSource<SendGridOptions>>();
        services.AddSingleton<IOptionsChangeTokenSource<RabbitMqOptions>, CredentialOptionsChangeTokenSource<RabbitMqOptions>>();

        services.AddFgsRefitClient<ISetupCredentialConfigurationClient>(
            configuration,
            $"{UserServiceCredentialClientOptions.SectionName}:BaseUrl");

        return services;
    }
}
