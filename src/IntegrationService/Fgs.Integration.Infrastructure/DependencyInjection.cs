using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Integration.Infrastructure.Database;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Integration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsIntegrationInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-integration-service", "DATABASE");

        services.AddDbContext<FgsIntegrationDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsIntegration,
                "FGS_INTEGRATION_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsIntegrationDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsIntegrationDbContext>();
        services.AddFgsDbContextReadyCheck<FgsIntegrationDbContext>();

        return services;
    }
}
