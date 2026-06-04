using Fgs.Audit.Infrastructure.Database;
using Fgs.Persistence.Extensions;
using Fgs.MultiTenancy;
using Fgs.Security.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Audit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAuditInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        var connectionString = configuration.GetConnectionString("FgsAudit")
            ?? throw new InvalidOperationException("ConnectionStrings:FgsAudit is required.");

        services.AddDbContext<FgsAuditDbContext>((_, options) =>
        {
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsAuditDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsAuditDbContext>();

        return services;
    }
}
