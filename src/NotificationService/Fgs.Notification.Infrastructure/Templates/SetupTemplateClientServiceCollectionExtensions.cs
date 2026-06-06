using Fgs.Contracts.Clients;
using Fgs.Foundation.Extensions;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Notification.Infrastructure.Templates;

public static class SetupTemplateClientServiceCollectionExtensions
{
    public static IServiceCollection AddSetupTemplateClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFgsRefitClient<ISetupTemplateClient>(
            configuration,
            $"{SetupServiceClientOptions.SectionName}:BaseUrl",
            "http://setup-service:5004");

        return services;
    }
}
