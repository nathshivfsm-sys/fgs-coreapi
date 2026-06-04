using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Publisher.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsPublisherInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        _ = configuration.GetConnectionString("FgsPublisher");
        return services;
    }
}
