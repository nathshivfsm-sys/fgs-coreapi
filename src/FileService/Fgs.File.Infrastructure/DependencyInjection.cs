using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.File.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsFileInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFgsFoundation();
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        _ = configuration.GetConnectionString("FgsFile");
        return services;
    }
}
