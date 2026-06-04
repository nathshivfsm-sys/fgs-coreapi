using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Consumer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsConsumerInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        _ = configuration.GetConnectionString("FgsConsumer");
        return services;
    }
}
