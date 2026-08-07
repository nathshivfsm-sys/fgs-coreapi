using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Asset.Infrastructure.Database;

public sealed class FgsAssetDbContextDesignFactory : IDesignTimeDbContextFactory<FgsAssetDbContext>
{
    public FgsAssetDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_ASSET_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_ASSET_DB or run dotnet ef with --startup-project Fgs.Asset.API.");

        var options = new DbContextOptionsBuilder<FgsAssetDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsAssetDbContext.MigrationHistorySchema))
            .Options;

        return new FgsAssetDbContext(options, new Fgs.MultiTenancy.Persistence.DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Asset.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsAsset");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "AssetService", "Fgs.Asset.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsAsset");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
