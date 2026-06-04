using Fgs.Contracts.Clients;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Fgs.Notification.Infrastructure.Templates;

public static class SetupTemplateClientServiceCollectionExtensions
{
    public static IServiceCollection AddSetupTemplateClient(this IServiceCollection services)
    {
        services
            .AddRefitClient<ISetupTemplateClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<UserServiceCredentialClientOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

        return services;
    }
}
