using Fgs.Foundation.Extensions;
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
    {
        services.AddFgsFoundation();
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);

        var connectionString = FgsIntegrationConnectionString.ResolveRequired(configuration);
        services.AddDbContext<FgsIntegrationDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsIntegrationDbContext.MigrationHistorySchema);
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });

        return services;
    }
}
