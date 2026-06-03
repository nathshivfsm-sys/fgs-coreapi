using Fgs.Foundation.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Audit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAuditInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFgsFoundation();
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        _ = configuration.GetConnectionString("FgsAudit");
        return services;
    }
}
