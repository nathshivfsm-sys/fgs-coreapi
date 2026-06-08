using Fgs.Credentials.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Dispatch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsDispatchInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-dispatch-service";
                options.RequiredProviders = ["DATABASE"];
            });

        services.AddFgsApiSecurity(configuration);
        return services;
    }
}
