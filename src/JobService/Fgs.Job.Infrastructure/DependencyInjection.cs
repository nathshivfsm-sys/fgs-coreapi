using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Job.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsJobInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        var connectionString = configuration.GetConnectionString("FgsJob");
        return services;
    }
}
