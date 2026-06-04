using Fgs.Persistence.Extensions;
using Fgs.Integration.Infrastructure.Database;
using Fgs.Security.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Integration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsIntegrationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        var connectionString = FgsIntegrationConnectionString.ResolveRequired(configuration);
        services.AddDbContext<FgsIntegrationDbContext>((_, options) =>
        {
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsIntegrationDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsIntegrationDbContext>();

        return services;
    }
}
