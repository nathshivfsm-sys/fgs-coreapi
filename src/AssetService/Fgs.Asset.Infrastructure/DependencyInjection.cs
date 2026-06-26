using Fgs.Asset.Infrastructure.Database;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Asset.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAssetInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-asset-service", "DATABASE");

        services.AddDbContext<FgsAssetDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsAsset,
                "FGS_ASSET_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsAssetDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsAssetDbContext>();

        return services;
    }
}
