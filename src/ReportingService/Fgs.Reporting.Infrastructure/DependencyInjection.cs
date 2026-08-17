using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.Reporting.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Reporting.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsReportingInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-reporting-service", "DATABASE");

        services.AddFgsDbContext<FgsReportingDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsReporting,
                "FGS_REPORTING_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsReportingDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsReportingDbContext>();
        services.AddFgsDbContextReadyCheck<FgsReportingDbContext>();

        return services;
    }
}
