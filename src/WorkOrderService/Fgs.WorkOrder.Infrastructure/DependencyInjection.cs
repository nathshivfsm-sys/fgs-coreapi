using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.WorkOrder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsWorkOrderInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFgsFoundation();
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        var connectionString = configuration.GetConnectionString("FgsWorkOrder");
        return services;
    }
}
